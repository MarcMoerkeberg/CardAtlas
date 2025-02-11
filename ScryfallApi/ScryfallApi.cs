using ScryfallApi.Models;
using ScryfallApi.Models.ResponseWrappers;
using ScryfallApi.Models.Types;
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

		public async Task<IEnumerable<Card>> GetBulkData(BulkDataType bulkDataType)
		{
			if (bulkDataType is BulkDataType.NotImplemented)
			{
				throw new ArgumentException(string.Format(Errors.InvalidBulkDataType, bulkDataType), nameof(bulkDataType));
			}
			if (!bulkDataType.IsCardDataType())
			{
				throw new ArgumentException(string.Format(Errors.InvalidBulkDataTypeForCards, bulkDataType), nameof(bulkDataType));
			}

			BulkData cardBulkDataObject = await GetBulkDataByType(bulkDataType);
			List<Card> deserializedModels = new List<Card>();

			await foreach (Card model in GetBulkDataAsync<Card>(cardBulkDataObject))
			{
				deserializedModels.Add(model);
			}

			return deserializedModels;
		}

		public async IAsyncEnumerable<Card> GetBulkDataAsync(BulkDataType bulkDataType)
		{
			if (bulkDataType is BulkDataType.NotImplemented)
			{
				throw new ArgumentException(string.Format(Errors.InvalidBulkDataType, bulkDataType), nameof(bulkDataType));
			}
			if (!bulkDataType.IsCardDataType())
			{
				throw new ArgumentException(string.Format(Errors.InvalidBulkDataTypeForCards, bulkDataType), nameof(bulkDataType));
			}

			BulkData cardBulkDataObject = await GetBulkDataByType(bulkDataType);

			await foreach (Card model in GetBulkDataAsync<Card>(cardBulkDataObject))
			{
				yield return model;
			}
		}

		public async Task<IEnumerable<Ruling>> GetBulkData()
		{
			BulkData rulingsBulkObject = await GetBulkDataByType(BulkDataType.Rulings);
			List<Ruling> deserializedModels = new List<Ruling>();

			await foreach (Ruling model in GetBulkDataAsync<Ruling>(rulingsBulkObject))
			{
				deserializedModels.Add(model);
			}

			return deserializedModels;
		}

		public async IAsyncEnumerable<Ruling> GetBulkDataAsync()
		{
			BulkData rulingsBulkObject = await GetBulkDataByType(BulkDataType.Rulings);

			await foreach (Ruling ruling in GetBulkDataAsync<Ruling>(rulingsBulkObject))
			{
				yield return ruling;
			}
		}

		private async Task<BulkData> GetBulkDataByType(BulkDataType bulkDataType)
		{
			if (bulkDataType is BulkDataType.NotImplemented)
			{
				throw new ArgumentException($"Invalid BulkDataType: {bulkDataType}.", nameof(bulkDataType));
			}

			IEnumerable<BulkData> allBulkData = await GetBulkDataObjects();

			return allBulkData.Single(bulkData => bulkData.BulkDataType == bulkDataType);
		}

		private async Task<IEnumerable<BulkData>> GetBulkDataObjects()
		{
			var apiResponse = await _client.GetAsync("bulk-data");
			if (!apiResponse.IsSuccessStatusCode)
			{
				throw new HttpRequestException(Errors.ApiResponseError);
			}

			ListResponse<BulkData>? responseData = await apiResponse.Content.ReadFromJsonAsync<ListResponse<BulkData>>();

			return responseData is null
				? throw new Exception(Errors.DeserializationError)
				: responseData.Data;
		}

		private async IAsyncEnumerable<TModel> GetBulkDataAsync<TModel>(BulkData bulkDataObject) where TModel : class
		{
			await foreach (TModel model in GetDataFromBulkObject<TModel>(bulkDataObject))
			{
				yield return model;
			}
		}

		private async IAsyncEnumerable<TModel> GetDataFromBulkObject<TModel>(BulkData bulkDataObject) where TModel : class
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
