using CardAtlas.Server.Models.Data.Cards;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data;

public class Format
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

	[InverseProperty("Format")]
	public ICollection<CardLegality> CardLegalities { get; set; } = new HashSet<CardLegality>();

	//TODO: Down the line consider adding nullable userId to this, so users can create their own formats and search through them at-will
}
