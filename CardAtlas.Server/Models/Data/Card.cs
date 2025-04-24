using CardAtlas.Server.Models.Data.CardRelations;
using CardAtlas.Server.Models.Data.Image;
using CardAtlas.Server.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data;

public class Card : INameable
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long Id { get; set; }
	public Guid? ScryfallId { get; set; }

	[MinLength(1)]
	[MaxLength(150)]
	public required string Name { get; set; }

	[MinLength(1)]
	[MaxLength(800)]
	public string? OracleText { get; set; }

	[MinLength(1)]
	[MaxLength(60)]
	public required string TypeLine { get; set; }

	[MinLength(1)]
	[MaxLength(500)]
	public string? FlavorText { get; set; }

	[MinLength(3)]
	[MaxLength(50)]
	public string? ManaCost { get; set; }

	[Precision(8, 1)]
	public decimal? ConvertedManaCost { get; set; }

	[MinLength(1)]
	[MaxLength(3)]
	public string? Power { get; set; }

	[MinLength(1)]
	[MaxLength(3)]
	public string? Toughness { get; set; }

	[MinLength(1)]
	[MaxLength(3)]
	public string? Loyalty { get; set; }

	[MinLength(1)]
	[MaxLength(6)]
	public required string CollectorNumber { get; set; }

	public required DateOnly ReleaseDate { get; set; }
	public required bool IsOnReservedList { get; set; }
	public required bool CanBeFoundInBoosters { get; set; }
	public required bool IsDigitalOnly { get; set; }
	public required bool IsFullArt { get; set; }
	public required bool IsOversized { get; set; }
	public required bool IsPromo { get; set; }
	public required bool IsReprint { get; set; }
	public required bool IsTextless { get; set; }
	public required bool IsWotcOfficial { get; set; }

	[ForeignKey("SetId")]
	public Set Set { get; set; } = null!;
	public required int SetId { get; set; }

	[ForeignKey("ArtistId")]
	public Artist Artist { get; set; } = null!;
	public required int ArtistId { get; set; }

	[InverseProperty("Card")]
	public ICollection<CardImage> Images { get; set; } = new HashSet<CardImage>();

	[ForeignKey("RarityId")]
	public Rarity Rarity { get; set; } = null!;
	public required int RarityId { get; set; }

	[InverseProperty("Card")]
	public ICollection<CardPrice> Prices { get; set; } = new HashSet<CardPrice>();

	[ForeignKey("LanguageId")]
	public Language Language { get; set; } = null!;
	public required int LanguageId { get; set; }

	[MinLength(2)]
	[MaxLength(9)]
	public required string ColorIdentity { get; set; }
	[NotMapped]
	public IEnumerable<string> ColorIdentities => ColorIdentity.Split(',');

	[InverseProperty("Card")]
	public ICollection<CardKeyword> CardKeywords { get; set; } = new HashSet<CardKeyword>();

	[InverseProperty("Card")]
	public ICollection<CardPromoType> CardPromoTypes { get; set; } = new HashSet<CardPromoType>();

	[ForeignKey("FrameLayoutId")]
	public FrameLayout FrameLayout { get; set; } = null!;
	public required int FrameLayoutId { get; set; }

	[InverseProperty("Card")]
	public ICollection<CardLegality> Legalities { get; set; } = new HashSet<CardLegality>();

	[InverseProperty("Card")]
	public ICollection<CardPrintFinish> CardPrintFinishes { get; set; } = new HashSet<CardPrintFinish>();
	[NotMapped]
	private IEnumerable<PrintFinishType>? _printFinishes { get; set; }
	[NotMapped]
	public IEnumerable<PrintFinishType> PrintFinishes => _printFinishes ??= CardPrintFinishes.Select(finish => finish.PrintFinish.Type);

	[InverseProperty("Card")]
	public ICollection<CardGameType> GameTypes { get; set; } = new HashSet<CardGameType>();
	[NotMapped]
	private IEnumerable<GameKind>? _existsInGameTypes { get; set; }
	[NotMapped]
	public IEnumerable<GameKind> PrintedInGameTypes => _existsInGameTypes ??= GameTypes.Select(gameType => gameType.GameType.Type);

	[ForeignKey("ParentCardId")]
	public long? ParentCardId { get; set; }
	public Card? ParentCard { get; set; }

	[InverseProperty("ParentCard")]
	public ICollection<Card> ChildCards { get; set; } = new HashSet<Card>();
}
