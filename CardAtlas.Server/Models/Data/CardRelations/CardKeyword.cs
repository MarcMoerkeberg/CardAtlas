using CardAtlas.Server.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data.CardRelations;

public class CardKeyword : ICardRelateable
{
	[Key]
	public int Id { get; set; }

	[ForeignKey("CardId")]
	public long CardId { get; set; }
	public Card Card { get; set; } = null!;

	[ForeignKey("KeywordId")]
	public int KeywordId { get; set; }
	public Keyword Keyword { get; set; } = null!;
}

