using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.CardRelations;
using ApiCard = ScryfallApi.Models.Card;
using CardFace = ScryfallApi.Models.CardFace;

namespace CardAtlas.Server.Mappers;

public static class ArtistMapper
{
	/// <summary>
	/// Maps the artist information on <paramref name="apiCard"/> to new <see cref="Artist"/> entities.
	/// </summary>
	/// <returns>A <see cref="Artist"/> entity for each on the card. List may be empty if <paramref name="apiCard"/> have insufficient artist information.</returns>
	public static List<Artist> MapArtist(ApiCard apiCard)
	{
		List<Artist> artists = new();
		if (apiCard.ArtistIds is not { Length: > 0 } || string.IsNullOrWhiteSpace(apiCard.ArtistName)) return artists;

		string[] artistNames = apiCard.ArtistName.Split('&');

		for (int i = 0; i < apiCard.ArtistIds.Length; i++)
		{
			artists.Add(new Artist
			{
				Name = artistNames[i],
				ScryfallId = apiCard.ArtistIds[i],
				SourceId = (int)SourceType.Scryfall
			});
		}

		return artists;
	}

	/// <summary>
	/// Maps <paramref name="cardFaces"/> into <see cref="Artist"/> entities.
	/// </summary>
	/// <returns>A new list of <see cref="Artist"/> entities. The list may be empty if <paramref name="cardFaces"/> have insufficient artist information.</returns>
	public static List<Artist> MapArtist(IEnumerable<CardFace> cardFaces)
	{
		List<Artist> artists = new();
		if (!cardFaces.Any()) return artists;

		foreach (CardFace cardFace in cardFaces)
		{
			Artist? artist = MapArtist(cardFace);
			if (artist is null) continue;

			artists.Add(artist);
		}

		return artists;
	}

	/// <summary>
	/// Maps the artist information on <paramref name="cardFace"/> to a new <see cref="Artist"/>.
	/// </summary>
	/// <returns>A new <see cref="Artist"/> entity or null if <paramref name="cardFace"/> have insufficient artist information.</returns>
	public static Artist? MapArtist(CardFace cardFace)
	{
		if (cardFace.ArtistId is null || string.IsNullOrWhiteSpace(cardFace.ArtistName)) return null;

		return new Artist
		{
			ScryfallId = cardFace.ArtistId,
			Name = cardFace.ArtistName,
			SourceId = (int)SourceType.Scryfall
		};
	}

	/// <summary>
	/// Maps relations between <see cref="Artist"/> and <see cref="Card"/> entities.<br/>
	/// Creates a new <see cref="CardArtist"/> for every entity in <paramref name="artists"/>.
	/// </summary>
	/// <returns>A new list of <see cref="CardArtist"/> entities for each entry in <paramref name="artists"/>.</returns>
	public static List<CardArtist> MapCardArtist(IEnumerable<Artist> artists)
	{
		if (!artists.Any()) return new List<CardArtist>();

		return artists
			.Select(MapCardArtist)
			.ToList();
	}

	/// <summary>
	/// Maps relations between <see cref="Artist"/> and <see cref="Card"/> entities.<br/>
	/// Creates a new <see cref="CardArtist"/> for every entity in <paramref name="artists"/>.
	/// </summary>
	/// <returns>A new list of <see cref="CardArtist"/> entities for each entry in <paramref name="artists"/>.</returns>
	public static CardArtist MapCardArtist(Artist artist) =>
		new CardArtist
		{
			ArtistId = artist.Id,
			CardId = 0,
		};
}
