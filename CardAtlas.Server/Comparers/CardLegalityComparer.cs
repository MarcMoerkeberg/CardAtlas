using CardAtlas.Server.Models.Data.CardRelations;

namespace CardAtlas.Server.Comparers;

public class CardLegalityComparer : IEqualityComparer<CardLegality>
{
	public bool Equals(CardLegality? x, CardLegality? y)
	{
		if (x is null && y is null) return true;
		if (x is null || y is null) return false;

		return x.Id == y.Id
		&& x.CardId == y.CardId
		&& x.GameFormatId == y.GameFormatId
		&& x.LegalityId == y.LegalityId;
	}

	public int GetHashCode(CardLegality obj)
	{
		var hash = new HashCode();
		hash.Add(obj.Id);
		hash.Add(obj.CardId);
		hash.Add(obj.GameFormatId);
		hash.Add(obj.LegalityId);

		return hash.ToHashCode();
	}

}
