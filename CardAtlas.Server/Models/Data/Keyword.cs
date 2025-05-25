using CardAtlas.Server.Models.Data.CardRelations;
using CardAtlas.Server.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data;

public class Keyword : IIdable, INameable, ISourceable
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	[MinLength(1)]
	[MaxLength(50)]
	public required string Name { get; set; }

	[MinLength(1)]
	[MaxLength(250)]
	public string? Description { get; set; }

	[MinLength(1)]
	[MaxLength(100)]
	public string? ReminderText { get; set; }

	[InverseProperty("Keyword")]
	public ICollection<CardKeyword> CardKeywords { get; set; } = new HashSet<CardKeyword>();

	[ForeignKey("SourceId")]
	[DeleteBehavior(DeleteBehavior.Restrict)]
	public Source Source { get; set; } = null!;
	public required int SourceId { get; set; }
	[NotMapped]
	public SourceType SourceType => Source.Type;

	//TODO: Add nullable userId to this entity, so users can create their own keywords.
}
