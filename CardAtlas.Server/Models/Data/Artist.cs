using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data;

public class Artist
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	public Guid? ScryfallId { get; set; }

	[MinLength(1)]
	[MaxLength(100)]
	public required string Name { get; set; }

	[InverseProperty("Artist")]
	public ICollection<Card> Cards { get; set; } = new HashSet<Card>();

	[NotMapped]
	public const int DefaultId = -1;
}
