using CardAtlas.Server.Models.Data;

namespace CardAtlas.Server.Comparers;

public class CardComparer : IEqualityComparer<Card>
{
	public bool Equals(Card? x, Card? y)
	{
		if (x is null && y is null) return true;
		if (x is null || y is null) return false;

		return x.ScryfallId == y.ScryfallId
			&& string.Equals(x.Name, y.Name)
			&& string.Equals(x.OracleText, y.OracleText)
			&& string.Equals(x.TypeLine, y.TypeLine)
			&& string.Equals(x.FlavorText, y.FlavorText)
			&& string.Equals(x.ManaCost, y.ManaCost)
			&& x.ConvertedManaCost == y.ConvertedManaCost
			&& string.Equals(x.Power, y.Power)
			&& string.Equals(x.Toughness, y.Toughness)
			&& string.Equals(x.Loyalty, y.Loyalty)
			&& string.Equals(x.CollectorNumber, y.CollectorNumber)
			&& Equals(x.ReleaseDate, y.ReleaseDate)
			&& x.IsOnReservedList == y.IsOnReservedList
			&& x.CanBeFoundInBoosters == y.CanBeFoundInBoosters
			&& x.IsDigitalOnly == y.IsDigitalOnly
			&& x.IsFullArt == y.IsFullArt
			&& x.IsOversized == y.IsOversized
			&& x.IsPromo == y.IsPromo
			&& x.IsReprint == y.IsReprint
			&& x.IsTextless == y.IsTextless
			&& x.IsWotcOfficial == y.IsWotcOfficial
			&& string.Equals(x.ColorIdentity, y.ColorIdentity)
			&& x.RarityId == y.RarityId
			&& x.FrameLayoutId == y.FrameLayoutId
			&& x.LanguageId == y.LanguageId
			&& x.SetId == y.SetId
			&& x.ArtistId == y.ArtistId;
	}

	public int GetHashCode(Card obj)
	{
		var hash = new HashCode();
		hash.Add(obj.ScryfallId);
		hash.Add(obj.Name);
		hash.Add(obj.OracleText);
		hash.Add(obj.TypeLine);
		hash.Add(obj.FlavorText);
		hash.Add(obj.ManaCost);
		hash.Add(obj.ConvertedManaCost);
		hash.Add(obj.Power);
		hash.Add(obj.Toughness);
		hash.Add(obj.Loyalty);
		hash.Add(obj.CollectorNumber);
		hash.Add(obj.ReleaseDate);
		hash.Add(obj.IsOnReservedList);
		hash.Add(obj.CanBeFoundInBoosters);
		hash.Add(obj.IsDigitalOnly);
		hash.Add(obj.IsFullArt);
		hash.Add(obj.IsOversized);
		hash.Add(obj.IsPromo);
		hash.Add(obj.IsReprint);
		hash.Add(obj.IsTextless);
		hash.Add(obj.IsWotcOfficial);
		hash.Add(obj.ColorIdentity);
		hash.Add(obj.RarityId);
		hash.Add(obj.FrameLayoutId);
		hash.Add(obj.LanguageId);
		hash.Add(obj.SetId);
		hash.Add(obj.ArtistId);

		return hash.ToHashCode();
	}

}
