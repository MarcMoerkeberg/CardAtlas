using CardAtlas.Server.Services.Interfaces;
using ScryfallApi;
using ScryfallApi.Models.Types;
using ApiCard = ScryfallApi.Models.Card;

namespace CardAtlas.Server.Services;

public class ScryfallIngestionService : IScryfallIngestionService
{
	private readonly IScryfallApi _scryfallApi;
	private readonly IScryfallDataTransformer _scryfallDataTransformer;

	public ScryfallIngestionService(IScryfallApi scryfallApi, IScryfallDataTransformer scryfallDataTransformer)
	{
		_scryfallApi = scryfallApi;
		_scryfallDataTransformer = scryfallDataTransformer;
	}

	public async Task UpsertCardCollection()
	{
		await foreach (ApiCard card in _scryfallApi.GetBulkCardDataAsync(BulkDataType.AllCards))
		{
			_scryfallDataTransformer.UpsertCard(card);

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
