using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data;

public class Set
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	public Guid? ScryfallId { get; set; }

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
	public SetType SetType { get; set; } = null!;
	public required int SetTypeId { get; set; }
	
	[InverseProperty("Set")]
	public ICollection<Card> Cards { get; set; } = new HashSet<Card>();

	public int NumberOfCardsInSet { get; set; }
	public bool IsDigitalOnly { get; set; }
	public bool IsFoilOnly { get; set; }
	public bool IsNonFoilOnly { get; set; }
	public DateOnly? ReleaseDate { get; set; }

	[ForeignKey("SourceId")]
	[DeleteBehavior(DeleteBehavior.Restrict)]
	public Source Source { get; set; } = null!;
	public required int SourceId { get; set; }
	[NotMapped]
	public SourceType SourceType => Source.Type;

	//TODO: Add nullable userId to this entity, so users can create their own formats.
}
