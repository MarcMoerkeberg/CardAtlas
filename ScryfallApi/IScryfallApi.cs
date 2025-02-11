using ScryfallApi.Models;
using ScryfallApi.Models.Types;

namespace ScryfallApi;

public interface IScryfallApi
{
	public Task<IEnumerable<Card>> GetBulkData(BulkDataType dataType);
	public IAsyncEnumerable<Card> GetBulkDataAsync(BulkDataType dataType);
	public Task<IEnumerable<Ruling>> GetBulkData();
	public IAsyncEnumerable<Ruling> GetBulkDataAsync();
}
