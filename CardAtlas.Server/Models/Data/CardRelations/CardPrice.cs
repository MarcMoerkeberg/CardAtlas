using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data.Cards;

public class CardPrice
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long Id { get; set; }

	[Precision(8, 2)]
	public decimal? Price { get; set; }

	[Precision(8, 2)]
	public decimal? FoilPrice { get; set; }

	[ForeignKey("VendorId")]
	public Vendor Vendor { get; set; } = null!;
	public required int VendorId { get; set; }

	public required Uri PurchaseUri { get; set; }

	[ForeignKey("CardId")]
	public Card Card { get; set; } = null!;
	public required long CardId { get; set; }
}
