using CardAtlas.Server.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data.CardRelations;

public class CardPrice : ICardRelateable, IIdable<long>
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long Id { get; set; }

	[Precision(10, 2)]
	public decimal? Price { get; set; }

	[Precision(10, 2)]
	public decimal? FoilPrice { get; set; }

	[ForeignKey("VendorId")]
	public Vendor Vendor { get; set; } = null!;
	public required int VendorId { get; set; }

	public Uri? PurchaseUri { get; set; }

	[ForeignKey("CurrencyId")]
	public Currency Currency { get; set; } = null!;
	public required int CurrencyId { get; set; }

	[ForeignKey("CardId")]
	public Card Card { get; set; } = null!;
	public required long CardId { get; set; }
}
