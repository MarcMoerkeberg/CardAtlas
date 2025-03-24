using CardAtlas.Server.Models.Data.CardRelations;
using CardAtlas.Server.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data;

public class PromoType : INameable, ISourceable
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	[MinLength(1)]
	[MaxLength(50)]
	public required string Name { get; set; }

	[ForeignKey("SourceId")]
	public Source Source { get; set; } = null!;
	public required int SourceId { get; set; }
	[NotMapped]
	public SourceType SourceType => Source.Type;

	[InverseProperty("PromoType")]
	public ICollection<CardPromoType> PromoTypes { get; set; } = new HashSet<CardPromoType>();
}
