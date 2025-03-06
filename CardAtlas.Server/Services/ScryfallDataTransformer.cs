using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Services.Interfaces;
using ApiCard = ScryfallApi.Models.Card;
using CardFace = ScryfallApi.Models.CardFace;
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

			ColorIdentity = string.Join(',', apiCard.ColorIdentity),
			Keywords = string.Join(',', apiCard.Keywords),
			PromoTypes = string.Join(',', apiCard.PromoTypes ?? []),

			RarityId = (int)GetRarity(apiCard),
			FrameLayoutId = (int)GetFrameLayoutType(apiCard),
			
			SetId = GetOrCreateSet(apiCard).Id,
			ArtistId = GetOrCreateArtist(apiCard, cardName).Id,
			LanguageId = GetOrCreateLanguage(apiCard).Id,
			CardLegalityId = UpsertLegality(apiCard).Id,
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


	/// <summary>
	/// Gets the <see cref="Artist"/> from <paramref name="apiCard"/>.<br/>
	/// Creates a new <see cref="Artist"/> if no matching artist is found in the database.<br/>
	/// Returns the default <see cref="Artist"/> if no artist data is available in <paramref name="apiCard"/>.
	/// </summary>
	/// <param name="cardName">Name of current <see cref="ApiCard"/> or <see cref="CardFace"/>.<br/>Is used to determine which face to get artist data from, in case the card has multiple.</param>
	/// <returns>An <see cref="Artist"/> from the database.</returns>
	private async Task<Artist> GetOrCreateArtist(ApiCard apiCard, string cardName)
	{
		Artist artistFromCard = GetArtistFromCardData(apiCard, cardName);

		return artistFromCard.ScryfallId.HasValue
			? await _artistService.GetFromScryfallId(artistFromCard.ScryfallId.Value) ?? await _artistService.Create(artistFromCard)
			: await _artistService.Get(Artist.DefaultArtistId);

	}

	/// <summary>
	/// Returns an <see cref="Artist"/> object from the given <see cref="ApiCard"/> object.<br/>
	/// Properties on the object may be null or empty, depending on the data available in the <paramref name="apiCard"/> object.
	/// </summary>
	/// <param name="cardName">Name of current <see cref="ApiCard"/> or <see cref="CardFace"/>.<br/>Is used to determine which face to get artist data from, in case the card has multiple.</param>
	/// <returns>A new <see cref="Artist"/> populated with data from <paramref name="apiCard"/>.</returns>
	private static Artist GetArtistFromCardData(ApiCard apiCard, string cardName)
	{
		Guid? artistId;
		string? artistName;

		if (apiCard.ArtistIds is { Length: 1})
		{
			artistId = apiCard.ArtistIds[0];
			artistName = apiCard.ArtistName;
		}
		else if (apiCard.CardFaces is { Length: 1 })
		{
			artistId = apiCard.CardFaces[0].ArtistId;
			artistName = apiCard.CardFaces[0].Artist;
		}
		else
		{
			CardFace? matchingCardFace = apiCard.CardFaces?.FirstOrDefault(face => face.Name == cardName);
			
			artistId = matchingCardFace?.ArtistId;
			artistName = matchingCardFace?.Artist;
		}

		return new Artist
		{
			ScryfallId = artistId,
			Name = artistName ?? string.Empty,
		};
	}
}
