using System.Text.Json.Serialization;

namespace CardAtlas.Server.Models.Scryfall;

public class RelatedUris
{
	[JsonPropertyName("gatherer")]
	public Uri? Gatherer { get; set; }

	[JsonPropertyName("tcgplayer_infinite_articles")]
	public Uri? TcgPlayerArticles { get; set; }

	[JsonPropertyName("tcgplayer_infinite_decks")]
	public Uri? TcgPlayerDecks { get; set; }

	[JsonPropertyName("edhrec")]
	public Uri? EdhRec { get; set; }
}