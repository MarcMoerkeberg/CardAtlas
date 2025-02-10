using CardAtlas.Server.Models.Internal;
using CardAtlas.Server.Models.Scryfall;
using CardAtlas.Server.Models.Scryfall.Types;
using CardAtlas.Server.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace CardAtlas.Server.Services
{
	public class ScryfallApi : IScryfallApi
	{
		private readonly HttpClient _client;

		public ScryfallApi(IOptions<AppSettings> appSettings)
		{
			_client = new HttpClient();
			_client.BaseAddress = new Uri("https://api.scryfall.com/");
			_client.DefaultRequestHeaders.Add("User-Agent", appSettings.Value.AppName);
		}

		public async Task<ScryfallCard[]> GetAllCardsAsync()
		{
			await GetBulkData(BulkDataType.AllCards);
			throw new NotImplementedException();
		}

		private async Task<BulkData> GetBulkData(BulkDataType bulkDataType)
		{
			IEnumerable<BulkData> allBulkData = await GetBulkData();
			
			return allBulkData.Single(bulkData => bulkData.BulkDataType == bulkDataType);
		}

		private async Task<IEnumerable<BulkData>> GetBulkData()
		{
			try
			{
				var apiResponse = await _client.GetAsync("bulk-data");
				if (!apiResponse.IsSuccessStatusCode)
				{
					throw new HttpRequestException("Failed to get bulk data from Scryfall api.");
				}

				ScryfallList<BulkData>? responseData = await apiResponse.Content.ReadFromJsonAsync<ScryfallList<BulkData>>();

				return responseData is null
					? throw new HttpRequestException("Failed deserializing bulk data content.") 
					: responseData.Data;
			}
			catch (Exception)
			{
				Console.WriteLine("Failed to get bulk data from Scryfall api.");
				throw;
			}
		}
	}
}
