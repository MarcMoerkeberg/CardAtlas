using ScryfallApi.Scryfall;
using ScryfallApi.Scryfall.Types;
using System.Net.Http.Json;
using System.Text.Json;

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

		public async Task<IEnumerable<TModel>> GetData<TModel>(BulkDataType dataType) where TModel : class
		{
			BulkData allCardsBulkDataObject = await GetBulkData(dataType);
			List<TModel> deserializedModels = new List<TModel>();

			await foreach (TModel model in GetScryfallData<TModel>(allCardsBulkDataObject))
			{
				deserializedModels.Add(model);
			}

			return deserializedModels;
		}

		public async IAsyncEnumerable<TModel> GetDataAsync<TModel>(BulkDataType dataType) where TModel : class
		{
			BulkData allCardsBulkDataObject = await GetBulkData(dataType);

			await foreach (TModel model in GetScryfallData<TModel>(allCardsBulkDataObject))
			{
				yield return model;
			}
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

		private async IAsyncEnumerable<TModel> GetScryfallData<TModel>(BulkData bulkDataObject) where TModel : class
		{
			Stream apiResponseStream = await _client.GetStreamAsync(bulkDataObject.DownloadUri);

			await foreach (TModel? model in JsonSerializer.DeserializeAsyncEnumerable<TModel>(apiResponseStream))
			{
				if (model is null) continue;

				yield return model;
			}
		}
	}
}
