using CardAtlas.Server.Models.Data;
using ApiCard = ScryfallApi.Models.Card;
using CardFace = ScryfallApi.Models.CardFace;
using FrameLayoutType = ScryfallApi.Models.Types.FrameLayoutType;
using ScryfallRarity = ScryfallApi.Models.Types.Rarity;

namespace CardAtlas.Server.Mappers;

public static class CardMapper
{
	/// <summary>
	/// Maps the card properties on <paramref name="apiCard"/> to a new <see cref="Card"/>.<br/>
	/// Prioritizes data from <paramref name="cardFace"/> if provided.
	/// </summary>
	/// <param name="cardFace">Prioritizes data from this if provided.</param>
	/// <returns>A new instance of <see cref="Card"/> populated with data from parameters.</returns>
	public static Card MapCard(ApiCard apiCard, Set set, Artist artist, CardFace? cardFace = null)
	{
		return new Card
		{
			Name = cardFace is not null
				? cardFace.Name
				: apiCard.Name,

			OracleText = cardFace is not null 
				? cardFace.OracleText 
				: apiCard.OracleText,

			TypeLine = cardFace is not null
				? cardFace.TypeLine ?? string.Empty
				: apiCard.TypeLine,

			FlavorText = cardFace is not null
				? cardFace.FlavorText
				: apiCard.FlavorText,

			ManaCost = cardFace is not null
				? NormalizeManaCost(cardFace.ManaCost)
				: NormalizeManaCost(apiCard.ManaCost),

			ConvertedManaCost = cardFace is not null
				? cardFace.ConvertedManaCost
				: apiCard.ConvertedManaCost,

			Power = cardFace is not null
				? cardFace.Power
				: apiCard.Power,

			Toughness = cardFace is not null
				? cardFace.Toughness
				: apiCard.Toughness,

			Loyalty = cardFace is not null
				? cardFace.Loyalty
				: apiCard.Loyalty,

			ScryfallId = apiCard.Id,
			CollectorNumber = apiCard.CollectorNumber,
			ReleaseDate = apiCard.ReleasedAt,
			IsOnReservedList = apiCard.IsOnReservedList,
			CanBeFoundInBoosters = apiCard.CanBeFoundInBoosterPacks,
			IsDigitalOnly = apiCard.IsOnlyDigitalPrint,
			IsFullArt = apiCard.IsFullArt,
			IsOversized = apiCard.CardIsOversized,
			IsPromo = apiCard.IsPromoPrint,
			IsReprint = apiCard.IsReprint,
			IsTextless = apiCard.IsTextlessPrint,
			IsWotcOfficial = true,

			ColorIdentity = string.Join(',', apiCard.ColorIdentity),

			RarityId = (int)GetRarity(apiCard),
			FrameLayoutId = (int)GetFrameLayoutType(apiCard),
			LanguageId = (int)GetLanguageType(apiCard),

			SetId = set.Id,
			ArtistId = artist.Id,
		};
	}

	private static string? NormalizeManaCost(string? manaCostString) => string.IsNullOrWhiteSpace(manaCostString) ? null : manaCostString;

	private static RarityType GetRarity(ApiCard apiCard)
	{
		return apiCard.Rarity switch
		{
			ScryfallRarity.Common => RarityType.Common,
			ScryfallRarity.Uncommon => RarityType.Uncommon,
			ScryfallRarity.Rare => RarityType.Rare,
			ScryfallRarity.Special => RarityType.Special,
			ScryfallRarity.Mythic => RarityType.Mythic,
			ScryfallRarity.Bonus => RarityType.Bonus,
			_ => RarityType.NotImplemented,
		};
	}

	private static FrameType GetFrameLayoutType(ApiCard apiCard)
	{
		return apiCard.FrameLayout switch
		{
			FrameLayoutType.Year1993 => FrameType.Year1993,
			FrameLayoutType.Year1997 => FrameType.Year1997,
			FrameLayoutType.Year2003 => FrameType.Year2003,
			FrameLayoutType.Year2015 => FrameType.Year2015,
			FrameLayoutType.Future => FrameType.Future,
			_ => FrameType.NotImplemented,
		};
	}

	private static LanguageType GetLanguageType(ApiCard apiCard)
	{
		return apiCard.LanguageCode switch
		{
			"en" => LanguageType.English,
			"es" => LanguageType.Spanish,
			"fr" => LanguageType.French,
			"de" => LanguageType.German,
			"it" => LanguageType.Italian,
			"pt" => LanguageType.Portuguese,
			"ja" => LanguageType.Japanese,
			"ko" => LanguageType.Korean,
			"ru" => LanguageType.Russian,
			"zhs" => LanguageType.SimplifiedChinese,
			"zht" => LanguageType.TraditionalChinese,
			"he" => LanguageType.Hebrew,
			"la" => LanguageType.Latin,
			"grc" => LanguageType.AncientGreek,
			"ar" => LanguageType.Arabic,
			"sa" => LanguageType.Sanskrit,
			"ph" => LanguageType.Phyrexian,
			_ => LanguageType.NotImplemented,
		};
	}

	/// <summary>
	/// Assigns all intrinsic properties from the <paramref name="source"/> onto the <paramref name="target"/>.<br/>
	/// These properties represent the core data of the <see cref="Card"/> (such as identifiers, text, numeric values, etc.)
	/// that are directly managed by the Card entity, excluding any navigational or derived properties.
	/// </summary>
	/// <param name="target">The entity being updated.</param>
	/// <param name="source">The properties of this object will be assigned to <paramref name="target"/>.</param>
	public static void MergeProperties(Card target, Card source)
	{
		target.Id = source.Id;
		target.ScryfallId = source.ScryfallId;
		target.Name = source.Name;
		target.OracleText = source.OracleText;
		target.TypeLine = source.TypeLine;
		target.FlavorText = source.FlavorText;
		target.ManaCost = source.ManaCost;
		target.ConvertedManaCost = source.ConvertedManaCost;
		target.Power = source.Power;
		target.Toughness = source.Toughness;
		target.Loyalty = source.Loyalty;
		target.CollectorNumber = source.CollectorNumber;
		target.ReleaseDate = source.ReleaseDate;
		target.IsOnReservedList = source.IsOnReservedList;
		target.CanBeFoundInBoosters = source.CanBeFoundInBoosters;
		target.IsDigitalOnly = source.IsDigitalOnly;
		target.IsFullArt = source.IsFullArt;
		target.IsOversized = source.IsOversized;
		target.IsPromo = source.IsPromo;
		target.IsReprint = source.IsReprint;
		target.IsTextless = source.IsTextless;
		target.IsWotcOfficial = source.IsWotcOfficial;
		target.SetId = source.SetId;
		target.ArtistId = source.ArtistId;
		target.RarityId = source.RarityId;
		target.LanguageId = source.LanguageId;
		target.ColorIdentity = source.ColorIdentity;
		target.FrameLayoutId = source.FrameLayoutId;
		target.ParentCardId = source.ParentCardId;
	}
}
