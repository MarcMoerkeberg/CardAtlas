using CardAtlas.Server.Models.Entities;

namespace CardAtlas.Server.Services.Interfaces;

public interface IOutboxService
{
	/// <summary>
	/// Processes the <paramref name="outboxMessages"/> based on their <see cref="MessageType"/>.
	/// Re-queues messages if the processing failed and retry-threshold is not exceeded.
	/// </summary>
	Task ProcessMessages(IEnumerable<OutboxMessage> outboxMessages);

	/// <summary>
	/// Processes the <paramref name="outboxMessage"/> based on it's <see cref="MessageType"/>.<br/>
	/// Re-queues the message if the processing failed and retry-threshold is not exceeded.
	/// </summary>
	Task ProcessMessage(OutboxMessage outboxMessage);
}