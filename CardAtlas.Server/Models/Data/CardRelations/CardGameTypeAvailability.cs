using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data.CardRelations;

public class CardGameTypeAvailability
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long Id { get; set; }

	[ForeignKey("Card")]
	public required long CardId { get; set; }
	public Card Card { get; set; } = null!;

	[ForeignKey("GameType")]
	public required int GameTypeId { get; set; }
	public GameType GameType { get; set; } = null!;
}