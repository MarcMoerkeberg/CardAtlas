using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data;

public class CardLegality
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long Id { get; set; }

	[ForeignKey("CardId")]
	public required Card Card { get; set; }
	public long CardId { get; set; }

	[ForeignKey("StandardLegalityId")]
	public required Legality Standard { get; set; }
	public int StandardLegalityId { get; set; }

	[ForeignKey("FutureLegalityId")]
	public required Legality Future { get; set; }
	public int FutureLegalityId { get; set; }

	[ForeignKey("HistoricLegalityId")]
	public required Legality Historic { get; set; }
	public int HistoricLegalityId { get; set; }

	[ForeignKey("TimelessLegalityId")]
	public required Legality Timeless { get; set; }
	public int TimelessLegalityId { get; set; }
	
	[ForeignKey("GladiatorLegalityId")]
	public required Legality Gladiator { get; set; }
	public int GladiatorLegalityId { get; set; }

	[ForeignKey("PioneerLegalityId")]
	public required Legality Pioneer { get; set; }
	public int PioneerLegalityId { get; set; }
	
	[ForeignKey("ExplorerLegalityId")]
	public required Legality Explorer { get; set; }
	public int ExplorerLegalityId { get; set; }

	[ForeignKey("ModernLegalityId")]
	public required Legality Modern { get; set; }
	public int ModernLegalityId { get; set; }

	[ForeignKey("LegacyLegalityId")]
	public required Legality Legacy { get; set; }
	public int LegacyLegalityId { get; set; }

	[ForeignKey("PauperLegalityId")]
	public required Legality Pauper { get; set; }
	public int PauperLegalityId { get; set; }

	[ForeignKey("VintageLegalityId")]
	public required Legality Vintage { get; set; }
	public int VintageLegalityId { get; set; }
	
	[ForeignKey("PennyLegalityId")]
	public required Legality Penny { get; set; }
	public int PennyLegalityId { get; set; }

	[ForeignKey("CommanderLegalityId")]
	public required Legality Commander { get; set; }
	public int CommanderLegalityId { get; set; }

	[ForeignKey("OatBreakerLegalityId")]
	public required Legality OatBreaker { get; set; }
	public int OatBreakerLegalityId { get; set; }

	[ForeignKey("StandardBrawlLegalityId")]
	public required Legality StandardBrawl { get; set; }
	public int StandardBrawlLegalityId { get; set; }

	[ForeignKey("BrawlLegalityId")]
	public required Legality Brawl { get; set; }
	public int BrawlLegalityId { get; set; }

	[ForeignKey("AlchemyLegalityId")]
	public required Legality Alchemy { get; set; }
	public int AlchemyLegalityId { get; set; }

	[ForeignKey("PauperCommanderLegalityId")]
	public required Legality PauperCommander { get; set; }
	public int PauperCommanderLegalityId { get; set; }

	[ForeignKey("DuelCommanderLegalityId")]
	public required Legality DuelCommander { get; set; }
	public int DuelCommanderLegalityId { get; set; }

	[ForeignKey("OldSchoolLegalityId")]
	public required Legality OldSchool { get; set; }
	public int OldSchoolLegalityId { get; set; }

	[ForeignKey("PreModernlLegalityId")]
	public required Legality PreModern { get; set; }
	public int PreModernlLegalityId { get; set; }

	[ForeignKey("PreDHLegalityId")]
	public required Legality PreDH { get; set; }
	public int PreDHLegalityId { get; set; }
}
