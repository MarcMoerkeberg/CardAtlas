using CardAtlas.Server.Models.Entities;

namespace CardAtlas.Server.Repositories.Interfaces
{
	public interface IOutboxRepository
	{
		/// <summary>
		/// Returns all <see cref="OutboxMessage"/> entities.
		/// </summary>
		/// <returns>All <see cref="OutboxMessage"/> entities in the db.</returns>
		Task<List<OutboxMessage>> GetAsync();

		/// <summary>
		/// Returns all <see cref="OutboxMessage"/> entities matching <paramref name="isProcessed"/> and <paramref name="maxRetries"/>.
		/// </summary>
		/// <param name="maxRetries">Skips entities with <see cref="OutboxMessage.RetryCount"/> that exceeds <paramref name="maxRetries"/>.</param>
		/// <returns>All <see cref="OutboxMessage"/> entities matching the specified pattern in the db.</returns>
		Task<List<OutboxMessage>> GetAsync(bool isProcessed, int maxRetries = 5);

		/// <summary>
		/// Finds <see cref="OutboxMessage"/> entities matching <paramref name="isProcessed"/> and <paramref name="maxRetries"/>.
		/// </summary>
		/// <param name="maxRetries">Skips entities with <see cref="OutboxMessage.RetryCount"/> that exceeds <paramref name="maxRetries"/>.</param>
		/// <param name="batchSize">Determines the maximum amount of entities returned in the list.</param>
		/// <returns>A list <see cref="OutboxMessage"/> entities matching the specified pattern in the db. List will contain at most x entries based on the <paramref name="batchSize"/>.</returns>
		Task<List<OutboxMessage>> GetAsync(bool isProcessed, int batchSize, int maxRetries = 5);

		/// <summary>
		/// Finds the <see cref="OutboxMessage"/> associated with the <paramref name="id"/>.
		/// </summary>
		/// <returns>The <see cref="OutboxMessage"/> associated with the <paramref name="id"/>.</returns>
		Task<OutboxMessage> GetAsync(Guid id);

		/// <summary>
		/// Finds all <see cref="OutboxMessage"/> entities associated with the <paramref name="ids"/>.
		/// </summary>
		/// <returns>All <see cref="OutboxMessage"/> entities associated with the <paramref name="ids"/>.</returns>
		Task<List<OutboxMessage>> GetAsync(IEnumerable<Guid> ids);

		/// <summary>
		/// Adds the <paramref name="outboxMessage"/> to the database.
		/// </summary>
		/// <returns>The added <see cref="OutboxMessage"/> with identity.</returns>
		Task<OutboxMessage> CreateAsync(OutboxMessage outboxMessage);

		/// <summary>
		/// Updates the <see cref="OutboxMessage"/> entity correlating with <paramref name="outboxMessage"/>.
		/// </summary>
		/// <returns>The updated <see cref="OutboxMessage"/>.</returns>
		Task<OutboxMessage> UpdateAsync(OutboxMessage outboxMessage);

		/// <summary>
		/// Updates the <see cref="OutboxMessage"/> entities correlating with the ones in <paramref name="outboxMessages"/>.
		/// </summary>
		/// <returns>The number of updated <see cref="OutboxMessage"/> entities.</returns>
		Task<int> UpdateAsync(IEnumerable<OutboxMessage> outboxMessages);
	}
}