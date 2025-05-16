using CardAtlas.Server.Models.Data;

namespace CardAtlas.Server.Comparers;

public class GameFormatComparer : IEqualityComparer<GameFormat>
{
	public bool Equals(GameFormat? x, GameFormat? y)
	{
		if (x is null && y is null) return true;
		if (x is null || y is null) return false;

		return x.Id == y.Id
			&& string.Equals(x.Name, y.Name)
			&& string.Equals(x.Description, y.Description)
			&& x.SourceId == y.SourceId;
	}

	public int GetHashCode(GameFormat obj)
	{
		var hash = new HashCode();
		hash.Add(obj.Id);
		hash.Add(obj.Name);
		hash.Add(obj.Description);
		hash.Add(obj.SourceId);

		return hash.ToHashCode();
	}

}
