using CardAtlas.Server.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data.CardRelations;

public class CardPromoType : ICardRelateable
{
	[Key]
	public long Id { get; set; }

	[ForeignKey("CardId")]
	public required long CardId { get; set; }
	public Card Card { get; set; } = null!;

	[ForeignKey("PromoTypeId")]
	public required int PromoTypeId { get; set; }
	public PromoType PromoType { get; set; } = null!;
}

