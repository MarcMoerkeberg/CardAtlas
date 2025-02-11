using ScryfallApi.Models;
using ScryfallApi.Models.Types;

namespace ScryfallApi;

public static class BulkDataGuards
{
	/// <summary>
	/// Returns true if <paramref name="dataType"/> corresponds to a bulk data object that returns <see cref="Card"/> objects<br/>
	/// See <see href="https://scryfall.com/docs/api/bulk-data">Scryfall API documentation</see> for more information.
	/// </summary>
	public static bool IsCardDataType(this BulkDataType dataType)
	{
		return dataType switch
		{
			BulkDataType.DefaultCards => true,
			BulkDataType.AllCards => true,
			BulkDataType.OracleCards => true,
			BulkDataType.UniqueArtwork => true,
			_ => false
		};
	}
}
