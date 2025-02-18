using CardAtlas.Server.Models.Data.Image;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data;

public class Card
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long Id { get; set; }
	public Guid? ScryfallId { get; set; }

	[MinLength(1)]
	[MaxLength(150)]
	public required string Name { get; set; }

	[MinLength(1)]
	[MaxLength(800)]
	public string? OracleText { get; set; }

	[MinLength(1)]
	[MaxLength(60)]
	public required string TypeLine { get; set; }

	[MinLength(1)]
	[MaxLength(500)]
	public string? FlavorText { get; set; }

	[MinLength(3)]
	[MaxLength(50)]
	public string? ManaCost { get; set; }

	[Precision(8, 1)]
	public decimal? ConvertedManaCost { get; set; }

	[MinLength(1)]
	[MaxLength(3)]
	public string? Power { get; set; }

	[MinLength(1)]
	[MaxLength(3)]
	public string? Toughness { get; set; }

	[ForeignKey("SetId")]
	public required Set Set { get; set; }
	public int SetId { get; set; }

	[ForeignKey("ArtistId")]
	public required Artist Artist { get; set; }
	public int ArtistId { get; set; }

	[InverseProperty("Card")]
	public ICollection<CardImage>? ImageUris { get; set; }

	[ForeignKey("RarityId")]
	public required Rarity Rarity { get; set; }
	public int RarityId { get; set; }

	[ForeignKey("CardPriceId")]
	public CardPrice? CardPrice { get; set; }
	public int? CardPriceId { get; set; }
}
