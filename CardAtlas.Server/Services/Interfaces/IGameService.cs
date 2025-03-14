using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.CardRelations;

namespace CardAtlas.Server.Services.Interfaces;

public interface IGameService
{
	/// <summary>
	/// Creates a new <see cref="CardGameType"/> entry in the database for each <paramref name="cardGameTypes"/>.
	/// </summary>
	/// <returns>The added <see cref="CardGameType"/> entities with identity.</returns>
	Task<IEnumerable<CardGameType>> CreateCardGameTypes(IEnumerable<CardGameType> cardGameTypes);

	/// <summary>
	/// Creates the provided <paramref name="format"/> to the database.
	/// </summary>
	/// <returns>The added <see cref="GameFormat"/> with identity.</returns>
	Task<GameFormat> CreateFormat(GameFormat format);

	/// <summary>
	/// Returns all <see cref="GameFormat"/> entities from the database.
	/// </summary>
	Task<IEnumerable<GameFormat>> GetFormats();

	/// <summary>
	/// Returns the <see cref="GameFormat"/> entity with the provided <paramref name="formatId"/>.
	/// Throws an <see cref="InvalidOperationException"/> if none, or more than one <see cref="GameFormat"/> entities is found.
	/// </summary>
	/// <exception cref="InvalidOperationException">Thrown if none, or more than one <see cref="GameFormat"/> entities is found.</exception>
	Task<GameFormat> GetFormat(int formatId);
}