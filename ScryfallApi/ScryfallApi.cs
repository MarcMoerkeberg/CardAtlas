using ScryfallApi.Models;
using ScryfallApi.Models.ResponseWrappers;
using ScryfallApi.Models.Types;
using System.Net.Http.Json;
using System.Text.Json;

namespace ScryfallApi
{
	public class ScryfallApi : IScryfallApi
	{
		public const int DefaultRateLimit = 100;
		public readonly int RateLimitInMilliseconds;

		private readonly HttpClient _client;
		private static readonly SemaphoreSlim _semaphore = new(1, 1);
		private static DateTime _lastRequestTime = DateTime.UtcNow;

		/// <param name="appName">The name of the application. Required as a User-Agent header when requesting the Scryfall API.</param>
		/// <param name="rateLimit">Rate limit in ms. Cannot be less than 100.</param>
		public ScryfallApi(string appName, int rateLimit = DefaultRateLimit)
		{
			_client = new HttpClient();
			_client.BaseAddress = new Uri("https://api.scryfall.com/");
			_client.DefaultRequestHeaders.Add("User-Agent", appName);

			RateLimitInMilliseconds = Math.Max(DefaultRateLimit, rateLimit);
		}


		/// <summary>
		/// Gets bulk data from the scryfall api as a stream and returns the objects once they are deserialized as <typeparamref name="TModel"/>.
		/// </summary>
		/// <typeparam name="TModel">The data is deserialized as this type.</typeparam>
		private async IAsyncEnumerable<TModel> GetBulkDataAsync<TModel>(BulkData bulkDataObject) where TModel : class
		{
			Stream apiResponseStream = await RateLimitAsync(() => _client.GetStreamAsync(bulkDataObject.DownloadUri));

			await foreach (TModel? model in JsonSerializer.DeserializeAsyncEnumerable<TModel>(apiResponseStream))
			{
				if (model is null) continue;

				yield return model;
			}
		}

		/// <summary>
		/// Rate limits <paramref name="asyncFunction"/> to ensure good citizenship. This is primarily used for calling the Scryfall API.<br/>
		/// For more details see the <see href="https://scryfall.com/docs/api">Scryfall API documentation</see>.
		/// </summary>
		/// <typeparam name="T">The expected returntype of <paramref name="asyncFunction"/>.</typeparam>
		/// <param name="asyncFunction">The function to execute once the ratelimit wait period has elapsed.</param>
		/// <returns>The result from <paramref name="asyncFunction"/>.</returns>
		private async Task<T> RateLimitAsync<T>(Func<Task<T>> asyncFunction)
		{
			await _semaphore.WaitAsync();
			try
			{
				int millisecondsSinceLastRequest = TimeElapsedSinceLastRequest();

				if (RateLimitInMilliseconds > millisecondsSinceLastRequest)
				{
					await Task.Delay(RateLimitInMilliseconds - millisecondsSinceLastRequest);
				}

				T asyncTaskResult = await asyncFunction();
				_lastRequestTime = DateTime.UtcNow;

				return asyncTaskResult;
			}
			finally
			{
				_semaphore.Release();
			}
		}

		/// <summary>
		/// Calculates the time since the last request in miliseconds.
		/// </summary>
		/// <returns>An int representing the time in milliseconds elapsed since last request.</returns>
		private static int TimeElapsedSinceLastRequest()
		{
			double timeSinceLastRequest = (DateTime.UtcNow - _lastRequestTime).TotalMilliseconds;
			return (int)timeSinceLastRequest;
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

		/// <summary>
		/// Returns a single specific type of bulk data object.
		/// </summary>
		/// <exception cref="ArgumentException"></exception>
		private async Task<BulkData> GetBulkDataByType(BulkDataType bulkDataType)
		{
			if (bulkDataType is BulkDataType.NotImplemented)
			{
				throw new ArgumentException($"Invalid BulkDataType: {bulkDataType}.", nameof(bulkDataType));
			}

			IEnumerable<BulkData> allBulkData = await GetBulkDataObjects();

			return allBulkData.Single(bulkData => bulkData.BulkDataType == bulkDataType);
		}

		/// <summary>
		/// Gets the bulk data objects from the api for fetching the bulk data.
		/// </summary>
		/// <returns>A list of <see cref="BulkData"/>.</returns>
		/// <exception cref="HttpRequestException"></exception>
		/// <exception cref="Exception"></exception>
		private async Task<IEnumerable<BulkData>> GetBulkDataObjects()
		{
			var apiResponse = await RateLimitAsync(() => _client.GetAsync("bulk-data"));
			if (!apiResponse.IsSuccessStatusCode)
			{
				throw new HttpRequestException(Errors.ApiResponseError);
			}

			ListResponse<BulkData>? responseData = await apiResponse.Content.ReadFromJsonAsync<ListResponse<BulkData>>();

			return responseData is null
				? throw new Exception(Errors.DeserializationError)
				: responseData.Data;
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

		public async Task<IEnumerable<Ruling>> GetBulkCardRulings()
		{
			BulkData rulingsBulkObject = await GetBulkDataByType(BulkDataType.Rulings);
			List<Ruling> deserializedModels = new List<Ruling>();

			await foreach (Ruling model in GetBulkDataAsync<Ruling>(rulingsBulkObject))
			{
				deserializedModels.Add(model);
			}

			return deserializedModels;
		}

		public async IAsyncEnumerable<Ruling> GetBulkCardRulingsAsync()
		{
			BulkData rulingsBulkObject = await GetBulkDataByType(BulkDataType.Rulings);

			await foreach (Ruling ruling in GetBulkDataAsync<Ruling>(rulingsBulkObject))
			{
				yield return ruling;
			}
		}

		public async Task<IEnumerable<Set>> GetSets()
		{
			var apiResponse = await RateLimitAsync(() => _client.GetAsync("sets"));
			if (!apiResponse.IsSuccessStatusCode)
			{
				throw new HttpRequestException(Errors.ApiResponseError);
			}

			var responseData = await apiResponse.Content.ReadFromJsonAsync<ListResponse<Set>>();
			if (responseData is null)
			{
				throw new Exception(Errors.DeserializationError);
			}

			return responseData.Data.Where(set => set is not null);
		}

		public Task<IEnumerable<CardSymbol>> GetCardSymbols()
		{
			throw new NotImplementedException();
		}

		public IAsyncEnumerable<CardSymbol> GetCardSymbolsAsync()
		{
			//Stream apiResponseStream = await RateLimitAsync(() => _client.GetStreamAsync("sets"));
			throw new NotImplementedException();
		}
	}
}
