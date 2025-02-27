using CardAtlas.Server.Models.Data;
using ApiCard = ScryfallApi.Models.Card;
using FrameLayoutType = ScryfallApi.Models.Types.FrameLayoutType;
using ScryfallRarity = ScryfallApi.Models.Types.Rarity;

namespace CardAtlas.Server.Mappers;

public static class CardMapper
{
	public static Card MapFromScryfallApi(ApiCard apiCard)
	{
		var mappedCard = new Card 
		{
			ScryfallId = apiCard.Id,
			Name = apiCard.Name,
			OracleText = apiCard.OracleText,
			TypeLine = apiCard.TypeLine,
			FlavorText = apiCard.FlavorText,
			ManaCost = apiCard.ManaCost,
			ConvertedManaCost = apiCard.ConvertedManaCost,
			Power = apiCard.Power,
			Toughness = apiCard.Toughness,
			Loyalty = apiCard.Loyalty,
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

			SetId = GetOrCreateSet(apiCard).Id,
			ArtistId = GetOrCreateArtist(apiCard).Id,
			RarityId = (int)GetRarity(apiCard),
			LanguageId = GetOrCreateLanguage(apiCard).Id,
			ColorIdentity = string.Join(',', apiCard.ColorIdentity),
			Keywords = string.Join(',', apiCard.Keywords),
			PromoTypes = string.Join(',', apiCard.PromoTypes ?? []),
			CardLegalityId = UpsertLegality(apiCard).Id,
			FrameLayoutId = (int)GetFrameLayoutType(apiCard),
			ParentCardId = UpsertParentCard(apiCard)?.Id,
		};

		UpsertImages(apiCard);
		UpsertPrices(apiCard);
		UpsertPrintFinishes(apiCard);
		UpsertGameTypes(apiCard);

		return mappedCard;
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

	private static void UpsertGameTypes(ApiCard apiCard)
	{
		throw new NotImplementedException();
	}

	private static void UpsertPrintFinishes(ApiCard apiCard)
	{
		throw new NotImplementedException();
	}

	private static void UpsertPrices(ApiCard apiCard)
	{
		throw new NotImplementedException();
	}

	private static void UpsertImages(ApiCard apiCard)
	{
		throw new NotImplementedException();
	}

	private static Card? UpsertParentCard(ApiCard apiCard)
	{
		throw new NotImplementedException();
	}

	private static CardLegality UpsertLegality(ApiCard apiCard)
	{
		throw new NotImplementedException();
	}

	private static Language GetOrCreateLanguage(ApiCard apiCard)
	{
		throw new NotImplementedException();
	}

	private static Set GetOrCreateSet(ApiCard apiCard)
	{
		throw new NotImplementedException();
	}

	private static Artist GetOrCreateArtist(ApiCard apiCard)
	{
		throw new NotImplementedException();
	}
}
