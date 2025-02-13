using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data;

public class Card
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long Id { get; set; }
	public Guid? ScryfallId { get; set; }

	[MaxLength(250)]
	[MinLength(1)]
	public required string Name { get; set; }
}
