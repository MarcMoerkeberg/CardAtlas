using CardAtlas.Server.Models.Data.CardRelations;
using CardAtlas.Server.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data;

public class PromoType : IIdable, INameable, ISourceable
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	[MinLength(1)]
	[MaxLength(50)]
	public required string Name { get; set; }

	[ForeignKey("SourceId")]
	[DeleteBehavior(DeleteBehavior.Restrict)]
	public Source Source { get; set; } = null!;
	public required int SourceId { get; set; }
	[NotMapped]
	public SourceType SourceType => Source.Type;

	[InverseProperty("PromoType")]
	public ICollection<CardPromoType> PromoTypes { get; set; } = new HashSet<CardPromoType>();
}
