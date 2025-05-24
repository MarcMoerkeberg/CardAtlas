using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data.CardRelations;

public class CardGamePlatform
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long Id { get; set; }

	[ForeignKey("Card")]
	public required long CardId { get; set; }
	public Card Card { get; set; } = null!;

	[ForeignKey("GamePlatform")]
	public required int GamePlatformId { get; set; }
	public GamePlatform GamePlatform { get; set; } = null!;
}