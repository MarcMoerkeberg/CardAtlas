using CardAtlas.Server.Extensions;
using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Services.Interfaces;
using ApiCard = ScryfallApi.Models.Card;
using FrameLayoutType = ScryfallApi.Models.Types.FrameLayoutType;
using ScryfallRarity = ScryfallApi.Models.Types.Rarity;

namespace CardAtlas.Server.Services;

public class ScryfallDataTransformer : IScryfallDataTransformer
{
	private readonly IArtistService _artistService;
	public ScryfallDataTransformer(IArtistService artistService)
	{
		_artistService = artistService;
	}

	public Card UpsertCard(ApiCard apiCard)
	{
		string cardName = apiCard.Name.Split("//").First();

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
			ArtistId = GetOrCreateArtist(apiCard, cardName).Id,
			RarityId = (int)GetRarity(apiCard),
			LanguageId = GetOrCreateLanguage(apiCard).Id,
			ColorIdentity = string.Join(',', apiCard.ColorIdentity),
			Keywords = string.Join(',', apiCard.Keywords),
			PromoTypes = string.Join(',', apiCard.PromoTypes ?? []),
			CardLegalityId = UpsertLegality(apiCard).Id,
			FrameLayoutId = (int)GetFrameLayoutType(apiCard),
			ParentCardId = UpsertParentCard(apiCard)?.Id,
		};

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





	//Assumed: If the api card has multiple ArtistIds it also has multiple card faces.
	//If it has multiple card faces, we should create/lookup each cardface and use the cardname to identify which id we should return.

	private async Task<Artist> GetOrCreateArtist(ApiCard apiCard, string cardName)
	{
		(Guid? artistId, string artistName) = GetArtistScryfallId(apiCard, cardName);

		var newArtist = new Artist
		{
			ScryfallId = artistId,
			Name = artistName,
		};

		return artistId.HasValue
			? await _artistService.GetFromScryfallId(artistId.Value) ?? await _artistService.Create(newArtist)
			: await _artistService.Get(Artist.UnknownArtistId);

	}

	private static (Guid? artistId, string artistName)GetArtistScryfallId(ApiCard apiCard, string cardName)
	{
		Guid? artistId;
		string? artistName = string.Empty;

		if(apiCard.ArtistIds.IsNullOrEmpty())
		{
			artistId = null;
		}
		else if (apiCard.ArtistIds.Length == 1)
		{
			artistId = apiCard.ArtistIds[0];
			artistName = apiCard.ArtistName;
		}
		else if (apiCard.CardFaces.IsNullOrEmpty())
		{
			artistId = null;
		}
		else if (apiCard.CardFaces.Length == 1)
		{
			artistId = apiCard.CardFaces[0].ArtistId;
			artistName = apiCard.CardFaces[0].Artist;
		}
		else
		{
			ScryfallApi.Models.CardFace matchingCardFace = apiCard.CardFaces.First(face => face.Name == cardName);
			artistId = matchingCardFace.ArtistId;
			artistName = matchingCardFace.Artist;
		}

		return (artistId, artistName);
	}
}
