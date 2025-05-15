namespace CardAtlas.Server.Services.Interfaces;

public interface IScryfallIngestionService
{
	Task UpsertCardCollection();

	Task<int> UpsertSets();
}