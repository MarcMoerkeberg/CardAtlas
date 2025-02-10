using ScryfallApi.Scryfall;
using ScryfallApi.Scryfall.Types;
using System.Net.Http.Json;

namespace ScryfallApi
{
	public class ScryfallApi : IScryfallApi
	{
		private readonly HttpClient _client;

		public ScryfallApi(string appName)
		{
			_client = new HttpClient();
			_client.BaseAddress = new Uri("https://api.scryfall.com/");
			_client.DefaultRequestHeaders.Add("User-Agent", appName);
		}

		public async Task<ScryfallCard[]> GetAllCardsAsync()
		{
			BulkData allCardsBulkDataObject = await GetBulkData(BulkDataType.AllCards);
			await GetScryfallData<ScryfallCard>(allCardsBulkDataObject);
			throw new NotImplementedException();
		}

		private async Task<BulkData> GetBulkData(BulkDataType bulkDataType)
		{
			IEnumerable<BulkData> allBulkData = await GetBulkData();

			return allBulkData.Single(bulkData => bulkData.BulkDataType == bulkDataType);
		}

		private async Task<IEnumerable<BulkData>> GetBulkData()
		{
			var apiResponse = await _client.GetAsync("bulk-data");
			if (!apiResponse.IsSuccessStatusCode)
			{
				throw new HttpRequestException(Errors.ApiResponseError);
			}

			ScryfallList<BulkData>? responseData = await apiResponse.Content.ReadFromJsonAsync<ScryfallList<BulkData>>();

			return responseData is null
				? throw new Exception(Errors.DeserializationError)
				: responseData.Data;
		}

		private async Task<IEnumerable<TModel>> GetScryfallData<TModel>(BulkData bulkDataObject) where TModel : class
		{
			try
			{
				var apiResponse = await _client.GetAsync(bulkDataObject.DownloadUri);
				if (!apiResponse.IsSuccessStatusCode)
				{
					throw new HttpRequestException(Errors.ApiResponseError);
				}

				var responseData = apiResponse.Content.ReadFromJsonAsync<IEnumerable<TModel>>();
			}
			catch (Exception)
			{
				throw;
			}

			throw new NotImplementedException();
		}

	}
}
