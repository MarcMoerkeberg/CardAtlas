using CardAtlas.Server.Mappers;
using CardAtlas.Server.Services.Interfaces;
using ScryfallApi;
using ScryfallApi.Models.Types;
using ApiCard = ScryfallApi.Models.Card;

namespace CardAtlas.Server.Services;

public class ScryfallIngestionService : IScryfallIngestionService
{
	private readonly IScryfallApi _scryfallApi;

	public ScryfallIngestionService(IScryfallApi scryfallApi)
	{
		_scryfallApi = scryfallApi;
	}

	public async Task UpsertCardCollection()
	{
		await foreach (ApiCard card in _scryfallApi.GetBulkCardDataAsync(BulkDataType.AllCards))
		{
			var mappedResult = CardMapper.MapFromScryfallApi(card);
		}
	}
}
