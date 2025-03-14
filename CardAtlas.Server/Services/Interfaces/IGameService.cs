using CardAtlas.Server.Models.Data.CardRelations;

namespace CardAtlas.Server.Services.Interfaces;

public interface IGameService
{
	/// <summary>
	/// Creates a new <see cref="CardGameType"/> entry in the database for each <paramref name="cardGameTypes"/>.
	/// </summary>
	/// <returns>The added <see cref="CardGameType"/> entities with identity.</returns>
	Task<IEnumerable<CardGameType>> CreateCardGameTypes(IEnumerable<CardGameType> cardGameTypes);
}