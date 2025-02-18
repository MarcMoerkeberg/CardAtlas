using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data;

public class Set
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	public Guid ScryfallId { get; set; }

	[MinLength(1)]
	[MaxLength(100)]
	public required string Name { get; set; }
	
	[MinLength(3)]
	[MaxLength(6)]
	public required string Code { get; set; }	
	
	[MinLength(3)]
	[MaxLength(6)]
	public string? MtgoCode { get; set; }
	
	[MinLength(3)]
	[MaxLength(6)]
	public string? ArenaCode { get; set; }
	
	[MinLength(3)]
	[MaxLength(6)]
	public string? ParentSetCode { get; set; }

	[MinLength(3)]
	[MaxLength(6)]
	public string? Block { get; set; }

	[MinLength(3)]
	[MaxLength(6)]
	public string? BlockCode { get; set; }

	[ForeignKey("SetTypeId")]
	public required SetType SetType { get; set; }
	public int SetTypeId { get; set; }
	
	[InverseProperty("Set")]
	public required ICollection<Card> Cards { get; set; }
	
	public int NumberOfCardsInSet { get; set; }
	public bool IsDigitalOnly { get; set; }
	public bool IsFoilOnly { get; set; }
	public bool IsNonFoilOnly { get; set; }
	public DateOnly? ReleaseDate { get; set; }
}
