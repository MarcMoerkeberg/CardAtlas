using CardAtlas.Server.Models.Data;

namespace CardAtlas.Server.Comparers;

public class SetComparer : IEqualityComparer<Set>
{
	public bool Equals(Set? x, Set? y)
	{
		if (x is null && y is null) return true;
		if (x is null || y is null) return false;

		return x.Id == y.Id
			&& x.ScryfallId == y.ScryfallId
			&& string.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase)
			&& string.Equals(x.Code, y.Code)
			&& string.Equals(x.MtgoCode, y.MtgoCode)
			&& string.Equals(x.ArenaCode, y.ArenaCode)
			&& string.Equals(x.ParentSetCode, y.ParentSetCode)
			&& string.Equals(x.Block, y.Block)
			&& string.Equals(x.BlockCode, y.BlockCode)
			&& x.SetTypeId == y.SetTypeId
			&& x.NumberOfCardsInSet == y.NumberOfCardsInSet
			&& x.IsDigitalOnly == y.IsDigitalOnly
			&& x.IsFoilOnly == y.IsFoilOnly
			&& x.IsNonFoilOnly == y.IsNonFoilOnly
			&& Equals(x.ReleaseDate, y.ReleaseDate)
			&& x.SourceId == y.SourceId;
	}

	public int GetHashCode(Set obj)
	{
		var hash = new HashCode();
		hash.Add(obj.Id);
		hash.Add(obj.ScryfallId);
		hash.Add(obj.Code);
		hash.Add(obj.Name);
		hash.Add(obj.MtgoCode);
		hash.Add(obj.ArenaCode);
		hash.Add(obj.ParentSetCode);
		hash.Add(obj.Block);
		hash.Add(obj.BlockCode);
		hash.Add(obj.SetTypeId);
		hash.Add(obj.NumberOfCardsInSet);
		hash.Add(obj.IsDigitalOnly);
		hash.Add(obj.IsFoilOnly);
		hash.Add(obj.IsNonFoilOnly);
		hash.Add(obj.ReleaseDate);
		hash.Add(obj.SourceId);

		return hash.ToHashCode();
	}

}
