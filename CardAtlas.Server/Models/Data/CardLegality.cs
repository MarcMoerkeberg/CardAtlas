using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data;

public class CardLegality
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long Id { get; set; }

	[ForeignKey("CardId")]
	[DeleteBehavior(DeleteBehavior.Restrict)]
	public Card Card { get; set; } = null!;
	public required long CardId { get; set; }

	[ForeignKey("StandardLegalityId")]
	[DeleteBehavior(DeleteBehavior.Restrict)]
	public Legality Standard { get; set; } = null!;
	public required int StandardLegalityId { get; set; }

	[ForeignKey("FutureLegalityId")]
	[DeleteBehavior(DeleteBehavior.Restrict)]
	public Legality Future { get; set; } = null!;
	public required int FutureLegalityId { get; set; }

	[ForeignKey("HistoricLegalityId")]
	[DeleteBehavior(DeleteBehavior.Restrict)]
	public Legality Historic { get; set; } = null!;
	public required int HistoricLegalityId { get; set; }

	[ForeignKey("TimelessLegalityId")]
	[DeleteBehavior(DeleteBehavior.Restrict)]
	public Legality Timeless { get; set; } = null!;
	public required int TimelessLegalityId { get; set; }
	
	[ForeignKey("GladiatorLegalityId")]
	[DeleteBehavior(DeleteBehavior.Restrict)]
	public Legality Gladiator { get; set; } = null!;
	public required int GladiatorLegalityId { get; set; }

	[ForeignKey("PioneerLegalityId")]
	[DeleteBehavior(DeleteBehavior.Restrict)]
	public Legality Pioneer { get; set; } = null!;
	public required int PioneerLegalityId { get; set; }
	
	[ForeignKey("ExplorerLegalityId")]
	[DeleteBehavior(DeleteBehavior.Restrict)]
	public Legality Explorer { get; set; } = null!;
	public required int ExplorerLegalityId { get; set; }

	[ForeignKey("ModernLegalityId")]
	[DeleteBehavior(DeleteBehavior.Restrict)]
	public Legality Modern { get; set; } = null!;
	public required int ModernLegalityId { get; set; }

	[ForeignKey("LegacyLegalityId")]
	[DeleteBehavior(DeleteBehavior.Restrict)]
	public Legality Legacy { get; set; } = null!;
	public required int LegacyLegalityId { get; set; }

	[ForeignKey("PauperLegalityId")]
	[DeleteBehavior(DeleteBehavior.Restrict)]
	public Legality Pauper { get; set; } = null!;
	public required int PauperLegalityId { get; set; }

	[ForeignKey("VintageLegalityId")]
	[DeleteBehavior(DeleteBehavior.Restrict)]
	public Legality Vintage { get; set; } = null!;
	public required int VintageLegalityId { get; set; }
	
	[ForeignKey("PennyLegalityId")]
	[DeleteBehavior(DeleteBehavior.Restrict)]
	public Legality Penny { get; set; } = null!;
	public required int PennyLegalityId { get; set; }

	[ForeignKey("CommanderLegalityId")]
	[DeleteBehavior(DeleteBehavior.Restrict)]
	public Legality Commander { get; set; } = null!;
	public required int CommanderLegalityId { get; set; }

	[ForeignKey("OatBreakerLegalityId")]
	[DeleteBehavior(DeleteBehavior.Restrict)]
	public Legality OatBreaker { get; set; } = null!;
	public required int OatBreakerLegalityId { get; set; }

	[ForeignKey("StandardBrawlLegalityId")]
	[DeleteBehavior(DeleteBehavior.Restrict)]
	public Legality StandardBrawl { get; set; } = null!;
	public required int StandardBrawlLegalityId { get; set; }

	[ForeignKey("BrawlLegalityId")]
	[DeleteBehavior(DeleteBehavior.Restrict)]
	public Legality Brawl { get; set; } = null!;
	public required int BrawlLegalityId { get; set; }

	[ForeignKey("AlchemyLegalityId")]
	[DeleteBehavior(DeleteBehavior.Restrict)]
	public Legality Alchemy { get; set; } = null!;
	public required int AlchemyLegalityId { get; set; }

	[ForeignKey("PauperCommanderLegalityId")]
	[DeleteBehavior(DeleteBehavior.Restrict)]
	public Legality PauperCommander { get; set; } = null!;
	public required int PauperCommanderLegalityId { get; set; }

	[ForeignKey("DuelCommanderLegalityId")]
	[DeleteBehavior(DeleteBehavior.Restrict)]
	public Legality DuelCommander { get; set; } = null!;
	public required int DuelCommanderLegalityId { get; set; }

	[ForeignKey("OldSchoolLegalityId")]
	[DeleteBehavior(DeleteBehavior.Restrict)]
	public Legality OldSchool { get; set; } = null!;
	public required int OldSchoolLegalityId { get; set; }

	[ForeignKey("PreModernlLegalityId")]
	[DeleteBehavior(DeleteBehavior.Restrict)]
	public Legality PreModern { get; set; } = null!;
	public required int PreModernlLegalityId { get; set; }

	[ForeignKey("PreDHLegalityId")]
	[DeleteBehavior(DeleteBehavior.Restrict)]
	public Legality PreDH { get; set; } = null!;
	public required int PreDHLegalityId { get; set; }
}
