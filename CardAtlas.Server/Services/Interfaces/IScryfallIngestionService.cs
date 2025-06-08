using CardAtlas.Server.Models.Data;

namespace CardAtlas.Server.Services.Interfaces;

public interface IScryfallIngestionService
{
	/// <summary>
	/// Updates all pre-existing official wotc <see cref="Set"/> entities and adds any new ones.
	/// </summary>
	/// <returns>The number of affected rows/entities.</returns>
	Task<int> UpsertSets();

	/// <summary>
	/// Creates new and updates existing official wotc <see cref="Card"/> entities and their related data.<br/>
	/// Related entities consists of <see cref="Artist"/>, <see cref="Set"/> etc.
	/// </summary>
	Task UpsertCardCollection();
}