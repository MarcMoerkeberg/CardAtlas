using ScryfallApi.Scryfall.Types;

namespace ScryfallApi
{
	public interface IScryfallApi
	{
		public Task<IEnumerable<TModel>> GetData<TModel>(BulkDataType dataType) where TModel : class;
		IAsyncEnumerable<TModel> GetDataAsync<TModel>(BulkDataType dataType) where TModel : class;
	}
}
