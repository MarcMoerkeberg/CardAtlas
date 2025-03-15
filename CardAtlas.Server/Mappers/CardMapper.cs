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
	/// Maps the card properties on <paramref name="apiCard"/> to a new <see cref="Card"/>.<br/>
	/// Prioritizes data from <paramref name="cardFace"/> if provided.
	/// </summary>
	/// <param name="cardFace">Prioritizes data from this if provided.</param>
	/// <returns>A new instance of <see cref="Card"/> populated with data from parameters.</returns>
	public static Card MapCard(ApiCard apiCard, Set set, Artist artist, CardFace? cardFace = null)
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

	public static HashSet<CardLegality> MapCardLegalities(long cardId, ApiCard apiCard, HashSet<GameFormat> gameFormats)
	{
		var legalities = new HashSet<CardLegality>();
		if (apiCard.ScryfallLegalities is { Count: 0 } || gameFormats is { Count: 0 }) return legalities;

		foreach (var (key, value) in apiCard.ScryfallLegalities)
		{
			GameFormat? gameFormat = gameFormats.GetWithName(key);
			if (gameFormat is null) continue;

			legalities.Add(new CardLegality
			{
				CardId = cardId,
				GameFormatId = gameFormat.Id,
				LegalityId = (int)GetLegalityType(value),
			});
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
}
