using CardAtlas.Server.Extensions;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.CardRelations;
using ApiCard = ScryfallApi.Models.Card;
using CardFace = ScryfallApi.Models.CardFace;
using FrameLayoutType = ScryfallApi.Models.Types.FrameLayoutType;
using ScryfallPrintFinish = ScryfallApi.Models.Types.Finish;
using ScryfallRarity = ScryfallApi.Models.Types.Rarity;

namespace CardAtlas.Server.Mappers;

public static class CardMapper
{
	/// <summary>
	/// Maps the <paramref name="apiCard"/> into a new list of <see cref="Card"/> entities.
	/// </summary>
	public static List<Card> MapCard(ApiCard apiCard, Set set) =>
		MapCards(apiCard, set)
		.AssignParent()
		.ToList();

	/// <summary>
	/// Maps a single <see cref="ApiCard"/> into multiple <see cref="Card"/> entities.
	/// </summary>
	/// <returns>An IEnumerable of <see cref="Card"/> entities mapped from the <paramref name="apiCard"/> and <paramref name="set"/>.</returns>
	private static IEnumerable<Card> MapCards(ApiCard apiCard, Set set)
	{
		if (apiCard.CardFaces is { Length: > 0 })
		{
			foreach (CardFace cardFace in apiCard.CardFaces)
			{
				yield return MapCard(apiCard, set, cardFace);
			}
		}
		else
		{
			yield return MapCard(apiCard, set, cardFace: null);
		}
	}

	/// <summary>
	/// Maps the card properties on <paramref name="apiCard"/> to a new <see cref="Card"/>.<br/>
	/// Prioritizes data from <paramref name="cardFace"/> if provided.
	/// </summary>
	/// <param name="cardFace">Prioritizes data from this if provided.</param>
	/// <returns>A new instance of <see cref="Card"/> populated with data from parameters.</returns>
	private static Card MapCard(ApiCard apiCard, Set set, CardFace? cardFace)
	{
		return new Card
		{
			Name = cardFace is null
				? apiCard.Name
				: cardFace.Name,

			OracleText = cardFace is null
				? apiCard.OracleText
				: cardFace.OracleText,

			TypeLine = cardFace is null
				? apiCard.TypeLine
				: cardFace.TypeLine ?? string.Empty,

			FlavorText = cardFace is null
				? apiCard.FlavorText
				: cardFace.FlavorText,

			ManaCost = NormalizeManaCost(cardFace is null
				? apiCard.ManaCost
				: cardFace.ManaCost
			),

			ConvertedManaCost = cardFace is null
				? apiCard.ConvertedManaCost
				: cardFace.ConvertedManaCost,

			Power = cardFace is null
				? apiCard.Power
				: cardFace.Power,

			Toughness = cardFace is null
				? apiCard.Toughness
				: cardFace.Toughness,

			Loyalty = cardFace is null
				? apiCard.Loyalty
				: cardFace.Loyalty,

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
			CreatedDate = DateTime.UtcNow.Truncate(TimeSpan.FromSeconds(1)),

			ColorIdentity = string.Join(',', apiCard.ColorIdentity),

			RarityId = (int)GetRarity(apiCard),
			FrameLayoutId = (int)GetFrameLayoutType(apiCard),
			LanguageId = (int)GetLanguageType(apiCard),

			SetId = set.Id,
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
		target.RarityId = source.RarityId;
		target.LanguageId = source.LanguageId;
		target.ColorIdentity = source.ColorIdentity;
		target.FrameLayoutId = source.FrameLayoutId;
		target.ParentCardId = source.ParentCardId;
	}

	/// <summary>
	/// Maps the print finishes from <paramref name="apiCard"/> to a collection of <see cref="PrintFinishType"/>
	/// </summary>
	public static HashSet<PrintFinishType> MapPrintFinishes(ApiCard apiCard)
	{
		var printFinishes = new HashSet<PrintFinishType>();
		if (apiCard.ComesInFinishes is null) return printFinishes;

		foreach (var finish in apiCard.ComesInFinishes)
		{
			printFinishes.Add(finish switch
			{
				ScryfallPrintFinish.Foil => PrintFinishType.Foil,
				ScryfallPrintFinish.Nonfoil => PrintFinishType.NonFoil,
				ScryfallPrintFinish.Etched => PrintFinishType.Etched,
				_ => PrintFinishType.NotImplemented,
			});
		}

		return printFinishes;
	}

	public static List<CardPrintFinish> MapCardPrintFinishes(ApiCard apiCard)
	{
		var printFinishes = new List<CardPrintFinish>();
		if (apiCard.ComesInFinishes is null) return printFinishes;

		foreach (ScryfallPrintFinish finish in apiCard.ComesInFinishes)
		{
			printFinishes.Add(finish switch
			{
				ScryfallPrintFinish.Foil => MapCardPrintFinish(PrintFinishType.Foil),
				ScryfallPrintFinish.Nonfoil => MapCardPrintFinish(PrintFinishType.NonFoil),
				ScryfallPrintFinish.Etched => MapCardPrintFinish(PrintFinishType.Etched),
				_ => MapCardPrintFinish(PrintFinishType.NotImplemented),
			});
		}

		return printFinishes;
	}

	private static CardPrintFinish MapCardPrintFinish(PrintFinishType printFinishType)
	{
		return new CardPrintFinish
		{
			PrintFinishId = (int)printFinishType,
			CardId = 0,
		};
	}

	/// <summary>
	/// Maps the legalities from <paramref name="apiCard"/> to <see cref="CardLegality"/> entities.<br/>
	/// Should have all relevant <see cref="GameFormat"/> entities in <paramref name="gameFormats"/> to map the legalities.
	/// </summary>
	/// <returns>A new <see cref="CardLegality"/> object for each format that exists in <paramref name="gameFormats"/> and <paramref name="apiCard"/>.</returns>
	public static List<(string formatName, CardLegality legality)> MapCardLegalities(ApiCard apiCard, IEnumerable<GameFormat> gameFormats)
	{
		List<(string formatName, CardLegality legality)> legalities = new();
		if (apiCard.ScryfallLegalities is { Count: 0 } || !gameFormats.Any()) return legalities;

		foreach ((string formatName, string legality) in apiCard.ScryfallLegalities)
		{
			GameFormat? gameFormat = gameFormats.FirstWithNameOrDefault(formatName);
			if (gameFormat is null) continue;

			legalities.Add((gameFormat.Name, new CardLegality
			{
				CardId = 0,
				GameFormatId = gameFormat.Id,
				LegalityId = (int)GetLegalityType(legality),
			}));
		}

		return legalities;
	}

	private static LegalityType GetLegalityType(string scryfallLegality)
	{
		return scryfallLegality switch
		{
			"legal" => LegalityType.Legal,
			"not_legal" => LegalityType.NotLegal,
			"restricted" => LegalityType.Restricted,
			"banned" => LegalityType.Banned,
			_ => LegalityType.NotImplemented
		};
	}

	/// <summary>
	/// Assigns all intrinsic properties from the <paramref name="source"/> onto the <paramref name="target"/>.<br/>
	/// These properties represent the core data of the <see cref="CardLegality"/> (such as identifiers, text, numeric values, etc.)
	/// that are directly managed by the CardLegality entity, excluding any navigational or derived properties.
	/// </summary>
	/// <param name="target">The entity being updated.</param>
	/// <param name="source">The properties of this object will be assigned to <paramref name="target"/>.</param>
	public static void MergeProperties(CardLegality target, CardLegality source)
	{
		target.Id = source.Id;
		target.CardId = source.CardId;
		target.GameFormatId = source.GameFormatId;
		target.LegalityId = source.LegalityId;
	}

	/// <summary>
	/// Returns a new list of <see cref="Keyword"/> entities populated with data from the <paramref name="apiCard"/>.
	/// </summary>
	public static IEnumerable<Keyword> MapKeywords(ApiCard apiCard)
	{
		if (apiCard.Keywords is not { Length: > 0 }) return Enumerable.Empty<Keyword>();

		return apiCard.Keywords
			.Select(keyword => new Keyword
			{
				Name = keyword.CapitalizeFirstLetter(),
				SourceId = (int)SourceType.Scryfall,
			})
			.ToList();
	}

	/// <summary>
	/// Maps the keywords from the <paramref name="apiCard"/> to <see cref="CardKeyword"/> entities.<br/>
	/// Should have all relevant <see cref="Keyword"/> entities in <paramref name="keywords"/> to map the relations correctly.
	/// </summary>
	/// <returns>A new <see cref="CardKeyword"/> object for each keyword on the <paramref name="apiCard"/> that matches an entry in <paramref name="keywords"/>.</returns>
	public static List<(string keywordName, CardKeyword cardKeyword)> MapCardKeywords(ApiCard apiCard, IEnumerable<Keyword> keywords)
	{
		List<(string keywordName, CardKeyword cardKeyword)> cardKeywords = new();
		if (apiCard.Keywords is not { Length: > 0 } || !keywords.Any()) return cardKeywords;

		foreach (string apiCardKeywordName in apiCard.Keywords)
		{
			Keyword? keyword = keywords.FirstWithNameOrDefault(apiCardKeywordName);
			if (keyword is null) continue;

			cardKeywords.Add((keyword.Name, new CardKeyword
			{
				CardId = 0,
				KeywordId = keyword.Id,
			}));
		}

		return cardKeywords;
	}

	/// <summary>
	/// Assigns all intrinsic properties from the <paramref name="source"/> onto the <paramref name="target"/>.<br/>
	/// These properties represent the core data of the <see cref="CardKeyword"/>
	/// that are directly managed by the CardKeyword entity, excluding any navigational or derived properties.
	/// </summary>
	/// <param name="target">The entity being updated.</param>
	/// <param name="source">The properties of this object will be assigned to <paramref name="target"/>.</param>
	public static void MergeProperties(CardKeyword target, CardKeyword source)
	{
		target.Id = source.Id;
		target.CardId = source.CardId;
		target.KeywordId = source.KeywordId;
	}

	/// <summary>
	/// Returns a new list of <see cref="PromoType"/> entities populated with data from the <paramref name="apiCard"/>.
	/// </summary>
	public static IEnumerable<PromoType> MapPromoTypes(ApiCard apiCard)
	{
		if (apiCard.PromoTypes is not { Length: > 0 }) return Enumerable.Empty<PromoType>();

		return apiCard.PromoTypes
			.Select(promoType => new PromoType
			{
				Name = promoType,
				SourceId = (int)SourceType.Scryfall,
			})
			.ToList();
	}

	/// <summary>
	/// Maps the promo types from the <paramref name="apiCard"/> to a new list of <see cref="CardPromoType"/> entities <br/>.
	/// Should have all relevant <see cref="PromoType"/> entities in <paramref name="promoTypes"/> to map the relations correctly.
	/// </summary>
	/// <returns>A new <see cref="CardPromoType"/> object for each promo type on the <paramref name="apiCard"/> that matches an entry in <paramref name="promoTypes"/>.</returns>
	public static List<(string promoTypeName, CardPromoType cardPromoType)> MapCardPromoTypes(ApiCard apiCard, IEnumerable<PromoType> promoTypes)
	{
		List<(string promoTypeName, CardPromoType cardPromoType)> cardPromoTypes = new();
		if (apiCard.PromoTypes is not { Length: > 0 } || !promoTypes.Any()) return cardPromoTypes;

		foreach (string apiCardPromoTypeName in apiCard.PromoTypes)
		{
			PromoType? promoType = promoTypes.FirstWithNameOrDefault(apiCardPromoTypeName);
			if (promoType is null) continue;

			cardPromoTypes.Add((promoType.Name, new CardPromoType
			{
				CardId = 0,
				PromoTypeId = promoType.Id,
			}));
		}

		return cardPromoTypes;
	}

	/// <summary>
	/// Assigns all intrinsic properties from the <paramref name="source"/> onto the <paramref name="target"/>.<br/>
	/// These properties represent the core data of the <see cref="CardPromoType"/>
	/// that are directly managed by the CardPromoType entity, excluding any navigational or derived properties.
	/// </summary>
	/// <param name="target">The entity being updated.</param>
	/// <param name="source">The properties of this object will be assigned to <paramref name="target"/>.</param>
	public static void MergeProperties(CardPromoType target, CardPromoType source)
	{
		target.Id = source.Id;
		target.CardId = source.CardId;
		target.PromoTypeId = source.PromoTypeId;
	}
}
