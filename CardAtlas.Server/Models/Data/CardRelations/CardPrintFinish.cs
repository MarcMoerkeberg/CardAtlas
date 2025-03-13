using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data.CardRelations;

public class CardPrintFinish
{
	[Key]
	public long Id { get; set; }

	[ForeignKey("CardId")]
	public required long CardId { get; set; }
	public Card Card { get; set; } = null!;

	[ForeignKey("PrintFinishId")]
	public required int PrintFinishId { get; set; }
	public PrintFinish PrintFinish { get; set; } = null!;
}