using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data;

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
	public required Vendor Vendor { get; set; }
	public int VendorId { get; set; }

	public required Uri PurchaseUri { get; set; }

	[ForeignKey("CardId")]
	public required Card Card { get; set; }
	public required long CardId { get; set; }
}
