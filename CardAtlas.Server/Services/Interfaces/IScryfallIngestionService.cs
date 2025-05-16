using CardAtlas.Server.Models.Data;

namespace CardAtlas.Server.Services.Interfaces;

public interface IScryfallIngestionService
{
	/// <summary>
	/// Updates all pre-existing official wotch <see cref="Set"/> entities and adds any new ones.
	/// </summary>
	/// <returns>The number of affected rows/entities.</returns>
	Task<int> UpsertSets();

	Task UpsertCardCollection();
}