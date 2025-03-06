using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data.Cards;

public class CardLegality
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long Id { get; set; }

	[ForeignKey("CardId")]
	public required long CardId { get; set; }
	public Card Card { get; set; } = null!;

	[ForeignKey("FormatId")]
	public required int FormatId { get; set; }
	public Format Format { get; set; } = null!;

	[ForeignKey("LegalityId")]
	public int LegalityId { get; set; }
	public Legality Legality { get; set; } = null!;
}
