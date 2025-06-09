using CardAtlas.Server.Models.Data.CardRelations;
using CardAtlas.Server.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data;

public class Artist : IIdable<int>
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	public Guid? ScryfallId { get; set; }

	[MinLength(1)]
	[MaxLength(100)]
	public required string Name { get; set; }

	[ForeignKey("SourceId")]
	[DeleteBehavior(DeleteBehavior.Restrict)]
	public Source Source { get; set; } = null!;
	public required int SourceId { get; set; }
	[NotMapped]
	public SourceType SourceType => Source.Type;

	[InverseProperty("Artist")]
	public ICollection<CardArtist> CardArtists { get; set; } = new HashSet<CardArtist>();

	[NotMapped]
	public const int DefaultId = -1;
}
