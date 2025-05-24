using CardAtlas.Server.Models.Data.CardRelations;

namespace CardAtlas.Server.Comparers;

public class CardGamePlatformComparer : IEqualityComparer<CardGamePlatform>
{
	public bool Equals(CardGamePlatform? x, CardGamePlatform? y)
	{
		if (x is null && y is null) return true;
		if (x is null || y is null) return false;

		return x.Id == y.Id
			&& string.Equals(x.GamePlatformId, y.GamePlatformId)
			&& string.Equals(x.CardId, y.CardId);
	}

	public int GetHashCode(CardGamePlatform obj)
	{
		var hash = new HashCode();
		hash.Add(obj.Id);
		hash.Add(obj.GamePlatformId);
		hash.Add(obj.CardId);

		return hash.ToHashCode();
	}

}
