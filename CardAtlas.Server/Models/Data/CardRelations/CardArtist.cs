using CardAtlas.Server.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data.CardRelations;

public class CardArtist : ICardRelateable
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long Id { get; set; }

	[ForeignKey("Card")]
	public required long CardId { get; set; }
	public Card Card { get; set; } = null!;

	[ForeignKey("Artist")]
	public required int ArtistId { get; set; }
	public Artist Artist { get; set; } = null!;
}
