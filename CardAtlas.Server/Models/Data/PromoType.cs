using CardAtlas.Server.Models.Data.CardRelations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data;

public class PromoType
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	
	[MinLength(1)]
	[MaxLength(50)]
	public required string Name { get; set; }

	[InverseProperty("PromoType")]
	public ICollection<CardPromoType> PromoTypes { get; set; } = new HashSet<CardPromoType>();
}
