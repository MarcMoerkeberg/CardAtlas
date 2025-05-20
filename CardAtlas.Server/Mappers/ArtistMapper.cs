using CardAtlas.Server.Models.Data;
using ApiCard = ScryfallApi.Models.Card;
using CardFace = ScryfallApi.Models.CardFace;

namespace CardAtlas.Server.Mappers;

public static class ArtistMapper
{
	/// <summary>
	/// Maps the artist information on <paramref name="apiCard"/> to a new <see cref="Artist"/>.
	/// </summary>
	/// <returns>A new <see cref="Artist"/> entity or null if <paramref name="apiCard"/> have insufficient artist information.</returns>
	public static Artist? MapArtist(ApiCard apiCard)
	{
		if(apiCard.ArtistIds is not { Length: > 0 } || string.IsNullOrWhiteSpace(apiCard.ArtistName)) return null;

		return new Artist
		{
			ScryfallId = apiCard.ArtistIds.First(),
			Name = apiCard.ArtistName,
		};
	}

	/// <summary>
	/// Maps the artist information on <paramref name="cardFace"/> to a new <see cref="Artist"/>.
	/// </summary>
	/// <returns>A new <see cref="Artist"/> entity or null if <paramref name="cardFace"/> have insufficient artist information.</returns>
	public static Artist? MapArtist(CardFace cardFace)
	{
		if (cardFace.ArtistId is null || string.IsNullOrWhiteSpace(cardFace.Artist)) return null;

		return new Artist
		{
			ScryfallId = cardFace.ArtistId,
			Name = cardFace.Artist,
		};
	}
}
