using CardAtlas.Server.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data.CardRelations;

public class CardLegality : ICardRelateable, IIdable<long>
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long Id { get; set; }

	[ForeignKey("CardId")]
	public required long CardId { get; set; }
	public Card Card { get; set; } = null!;

	[ForeignKey("GameFormatId")]
	public required int GameFormatId { get; set; }
	public GameFormat GameFormat { get; set; } = null!;

	[ForeignKey("LegalityId")]
	public int LegalityId { get; set; }
	public Legality Legality { get; set; } = null!;
}
