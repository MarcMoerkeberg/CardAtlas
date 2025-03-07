using CardAtlas.Server.Models.Data;
using ApiCard = ScryfallApi.Models.Card;
using CardFace = ScryfallApi.Models.CardFace;

namespace CardAtlas.Server.Mappers;

public static class ArtistMapper
{
	/// <summary>
	/// Maps the artist properties on <paramref name="apiCard"/> to a new <see cref="Artist"/>.<br/>
	/// Parameters may be null if no artist data is available.
	/// </summary>
	public static Artist MapArtist(ApiCard apiCard)
	{
		return new Artist
		{
			ScryfallId = apiCard.ArtistIds?[0],
			Name = apiCard.Name,
		};
	}

	/// <summary>
	/// Maps the artist properties on <paramref name="cardFace"/> to a new <see cref="Artist"/>.<br/>
	/// Parameters may be null or empty if no artist data is available.
	/// </summary>
	public static Artist MapArtist(CardFace cardFace)
	{
		return new Artist
		{
			ScryfallId = cardFace.ArtistId,
			Name = cardFace.Artist ?? string.Empty,
		};
	}
}
