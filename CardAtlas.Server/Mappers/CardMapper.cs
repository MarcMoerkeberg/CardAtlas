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

			OracleText = cardFace?.OracleText ?? apiCard.OracleText,

			TypeLine = cardFace is not null
				? cardFace.TypeLine ?? string.Empty
				: apiCard.TypeLine,

			FlavorText = cardFace is not null
				? cardFace?.FlavorText
				: apiCard.FlavorText,

			ManaCost = cardFace is not null
				? cardFace.ManaCost
				: apiCard.ManaCost,

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
}
