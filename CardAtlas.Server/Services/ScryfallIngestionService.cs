using CardAtlas.Server.Mappers;
using CardAtlas.Server.Models.Data;
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
			Card mappedCard = ScryfallMapper.FromApi(card);

			UpsertImages(card);
			UpsertPrices(card);
			UpsertPrintFinishes(card);
			UpsertGameTypes(card);
		}
	}


	private static void UpsertGameTypes(ApiCard apiCard)
	{
		throw new NotImplementedException();
	}

	private static void UpsertPrintFinishes(ApiCard apiCard)
	{
		throw new NotImplementedException();
	}

	private static void UpsertPrices(ApiCard apiCard)
	{
		throw new NotImplementedException();
	}

	private static void UpsertImages(ApiCard apiCard)
	{
		throw new NotImplementedException();
	}
}
