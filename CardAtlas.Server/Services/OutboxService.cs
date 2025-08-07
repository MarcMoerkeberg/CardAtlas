using CardAtlas.Server.Models.Entities;
using CardAtlas.Server.Models.Internal;
using CardAtlas.Server.Repositories.Interfaces;
using CardAtlas.Server.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Threading.Channels;

namespace CardAtlas.Server.Services;

public class OutboxService : IOutboxService
{
	private readonly IEmailService _emailSender;
	private readonly IOutboxRepository _outboxRepository;
	private readonly Channel<OutboxMessage> _outboxChannel;
	private int _maxRetryCount = 5;

	public OutboxService(
		IOptions<AppSettings> appSettings,
		IEmailService emailSender,
		IOutboxRepository outboxRepository,
		Channel<OutboxMessage> outboxChannel)
	{
		_maxRetryCount = appSettings.Value.OutboxSettings.MaxRetryCount;
		_emailSender = emailSender;
		_outboxRepository = outboxRepository;
		_outboxChannel = outboxChannel;
	}

	public async Task ProcessMessages(IEnumerable<OutboxMessage> outboxMessages)
	{
		List<OutboxMessage> processedMessages = new();

		foreach (OutboxMessage outboxMessage in outboxMessages)
		{
			processedMessages.Add(await DelegateMessageProcessing(outboxMessage));
		}

		try
		{
			await _outboxRepository.UpdateAsync(processedMessages);
		}
		catch (Exception)
		{
			//TODO: Add logging
		}

		List<OutboxMessage> messagesToRetry = processedMessages.Where(
			message => !message.IsProcessed &&
			message.RetryCount <= _maxRetryCount
		).ToList();

		foreach (OutboxMessage message in messagesToRetry)
		{
			await _outboxChannel.Writer.WriteAsync(message);
		}
	}

	public async Task ProcessMessage(OutboxMessage outboxMessage)
	{
		OutboxMessage processedMessage = await DelegateMessageProcessing(outboxMessage);

		try
		{
			await _outboxRepository.UpdateAsync(processedMessage);
		}
		catch (Exception)
		{
			//TODO: Add logging
		}

		if (!processedMessage.IsProcessed && processedMessage.RetryCount <= _maxRetryCount)
		{
			await _outboxChannel.Writer.WriteAsync(processedMessage);
		}
	}

	/// <summary>
	/// Delegates the <paramref name="outboxMessage"/> to it's processing service based on the message's <see cref="MessageType"/>.
	/// </summary>
	/// <returns>The <paramref name="outboxMessage"/> updated based on how the messageprocessing went.</returns>
	/// <exception cref="NotImplementedException"></exception>
	/// <exception cref="ArgumentException"></exception>
	private async Task<OutboxMessage> DelegateMessageProcessing(OutboxMessage outboxMessage)
	{
		return (MessageType)outboxMessage.MessageTypeId switch
		{
			MessageType.EmailConfirmation => await ProcessEmailMessage(outboxMessage),
			MessageType.NotImplemented => throw new NotImplementedException(),
			_ => throw new ArgumentException()
		};
	}

	/// <summary>
	/// Sends an email from the <paramref name="outboxMessage"/> payload.
	/// </summary>
	/// <returns>The <paramref name="outboxMessage"/> updated based on how the messageprocessing went.</returns>
	private async Task<OutboxMessage> ProcessEmailMessage(OutboxMessage outboxMessage)
	{
		OutboxEmail? outboxEmail = JsonSerializer.Deserialize<OutboxEmail>(outboxMessage.Payload);
		if (outboxEmail is null)
		{
			//TODO: Add logging

			outboxMessage.RetryCount++;
			return outboxMessage;
		}

		EmailResult emailResult = await _emailSender.SendEmailAsync(outboxEmail);

		outboxMessage.LastModifiedDate = DateTime.UtcNow;
		if (emailResult.Succeeded)
		{
			outboxMessage.IsProcessed = true;
		}
		else
		{
			//TODO: Add logging

			outboxMessage.RetryCount++;
		}

		return outboxMessage;
	}
}
