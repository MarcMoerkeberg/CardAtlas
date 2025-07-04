using CardAtlas.Server.Models.Data;

namespace CardAtlas.Server.Comparers;

public class ArtistComparer : IEqualityComparer<Artist>
{
	public bool Equals(Artist? x, Artist? y)
	{
		if (x is null && y is null) return true;
		if (x is null || y is null) return false;

		return x.ScryfallId == y.ScryfallId
			&& x.Id == y.Id
			&& string.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
	}

	public int GetHashCode(Artist obj)
	{
		var hash = new HashCode();
		hash.Add(obj.ScryfallId);
		hash.Add(obj.Id);
		hash.Add(obj.Name);

		return hash.ToHashCode();
	}
}
