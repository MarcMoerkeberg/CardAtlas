using CardAtlas.Server.Models.Data;

namespace CardAtlas.Server.Comparers;

public class PromoTypeComparer : IEqualityComparer<PromoType>
{
	public bool Equals(PromoType? x, PromoType? y)
	{
		if (x is null && y is null) return true;
		if (x is null || y is null) return false;

		return x.Id == y.Id
			&& x.SourceId == y.SourceId
			&& string.Equals(x.Name, y.Name)
			&& string.Equals(x.SourceId, y.SourceId);
	}

	public int GetHashCode(PromoType obj)
	{
		var hash = new HashCode();
		hash.Add(obj.Id);
		hash.Add(obj.Name);
		hash.Add(obj.SourceId);

		return hash.ToHashCode();
	}
}
