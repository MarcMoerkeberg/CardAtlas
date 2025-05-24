using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Internal;

namespace CardAtlas.Server.Repositories.Interfaces
{
	public interface IArtistRepository
	{
		/// <summary>
		/// Returns the <see cref="Artist"/> from the db with the specified <paramref name="artistId"/>.<br/>
		/// Throws an <see cref="InvalidOperationException"/> if no, or one or more <see cref="Artist"/> entities is found.
		/// </summary>
		/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
		Task<Artist> Get(int artistId);

		/// <summary>
		/// Returns the <see cref="Artist"/> from the db with the specified <paramref name="scryfallId"/>.
		/// </summary>
		/// <returns>The <see cref="Artist"/> with the specified <paramref name="scryfallId"/> or null if no match is found.</returns>
		/// <exception cref="InvalidOperationException">Is thrown if no or more than one entity with that id is found.</exception>
		Task<Artist?> Get(Guid scryfallId);

		/// <summary>
		/// Returns the <see cref="Artist"/> entities from the db with the specified <paramref name="scryfallIds"/>.
		/// </summary>
		/// <returns>A list of <see cref="Artist"/> entities with the specified <paramref name="scryfallIds"/>.</returns>
		Task<IEnumerable<Artist>> Get(IEnumerable<Guid> scryfallIds);

		/// <summary>
		/// Adds the provided <paramref name="artist"/> to the database.
		/// </summary>
		/// <returns>The added <see cref="Artist"/> with identity.</returns>
		Task<Artist> Create(Artist artist);

		/// <summary>
		/// Creates and updates <see cref="Artist"/> entities, based on the provided <paramref name="upsertionData"/>.
		/// </summary>
		/// <returns>The number of affected effected entities.</returns>
		Task<int> Upsert(UpsertContainer<Artist> upsertionData);
	}
}