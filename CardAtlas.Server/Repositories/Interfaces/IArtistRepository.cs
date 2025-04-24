using CardAtlas.Server.Models.Data;

namespace CardAtlas.Server.Repositories.Interfaces
{
	public interface IArtistRepository
	{
		/// <summary>
		/// Adds the provided <paramref name="artist"/> to the database.
		/// </summary>
		/// <returns>The added <see cref="Artist"/> with identity.</returns>
		Task<Artist> Create(Artist artist);

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
		Task<Artist?> GetFromScryfallId(Guid scryfallId);
	}
}