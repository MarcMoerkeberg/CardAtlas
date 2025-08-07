
using CardAtlas.Server.Models.Entities;
using CardAtlas.Server.Models.Internal;
using CardAtlas.Server.Repositories.Interfaces;
using CardAtlas.Server.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Threading.Channels;

namespace CardAtlas.Server.BackgroundServices;

public class OutboxChannelBackgroundService : BackgroundService
{
	private readonly int _maxRetryCount;
	private readonly Channel<OutboxMessage> _outboxChannel;
	private readonly IServiceScopeFactory _scopeFactory;

	public OutboxChannelBackgroundService(
		IOptions<AppSettings> appSettings,
		Channel<OutboxMessage> outboxChannel,
		IServiceScopeFactory scopeFactory)
	{
		_maxRetryCount = appSettings.Value.OutboxSettings.MaxRetryCount;
		_outboxChannel = outboxChannel;
		_scopeFactory = scopeFactory;
	}

	public override async Task StartAsync(CancellationToken cancellationToken)
	{
		using IServiceScope scope = _scopeFactory.CreateScope();
		IOutboxRepository outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();

		List<OutboxMessage> pendingMessages = await outboxRepository.GetAsync(
			isProcessed: false,
			maxRetries: _maxRetryCount
		);

		foreach (OutboxMessage message in pendingMessages)
		{
			await _outboxChannel.Writer.WriteAsync(message);
		}

		await base.StartAsync(cancellationToken);
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await foreach (OutboxMessage message in _outboxChannel.Reader.ReadAllAsync(stoppingToken))
		{
			try
			{
				using IServiceScope scope = _scopeFactory.CreateScope();
				IOutboxService outboxService = scope.ServiceProvider.GetRequiredService<IOutboxService>();

				await outboxService.ProcessMessage(message);
			}
			catch (OperationCanceledException)
			{
				//TODO: Add logging
				break;
			}
			catch (Exception)
			{
				//TODO: Add logging
			}
		}
	}
}
