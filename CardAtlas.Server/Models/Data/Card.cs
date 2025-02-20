using CardAtlas.Server.Models.Data.Image;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data;

public class Card
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

	[ForeignKey("SetId")]
	public Set Set { get; set; } = null!;
	public required int SetId { get; set; }

	[ForeignKey("ArtistId")]
	public Artist Artist { get; set; } = null!;
	public required int ArtistId { get; set; }

	[InverseProperty("Card")]
	public ICollection<CardImage>? ImageUris { get; set; }

	[ForeignKey("RarityId")]
	public Rarity Rarity { get; set; } = null!;
	public required int RarityId { get; set; }

	[InverseProperty("Card")]
	public ICollection<CardPrice>? CardPrices { get; set; }

	[ForeignKey("LanguageId")]
	public Language Language { get; set; } = null!;
	public required int LanguageId { get; set; }

	[MinLength(2)]
	[MaxLength(9)]
	public required string ColorIdentity { get; set; }
	[NotMapped]
	public IEnumerable<string> ColorIdentities => ColorIdentity.Split(',');

	[MinLength(1)]
	[MaxLength(100)]
	public string? Keywords { get; set; }
	[NotMapped]
	public IEnumerable<string>? KeywordList => Keywords?.Split(',');

	[ForeignKey("CardLegalityId")]
	public CardLegality Legality { get; set; } = null!;
	public required long CardLegalityId { get; set; }

	[MinLength(1)]
	[MaxLength(3)]
	public string? Loyalty { get; set; }

	[MinLength(1)]
	[MaxLength(6)]
	public required string CollectorNumber { get; set; }

	[ForeignKey("FrameLayoutId")]
	public FrameLayout FrameLayout { get; set; } = null!;
	public required int FrameLayoutId { get; set; }

	public ICollection<PrintFinish> PrintFinishes { get; set; } = new HashSet<PrintFinish>();
	[NotMapped]
	private IEnumerable<PrintFinishType>? _printedInFinishes { get; set; }
	[NotMapped]
	public IEnumerable<PrintFinishType> PrintedInFinishes => _printedInFinishes??= PrintFinishes.Select(finish => finish.Type);

	public ICollection<GameType> GameTypes { get; set; } = new HashSet<GameType>();
	[NotMapped]
	private IEnumerable<GameKind>? _existsInGameTypes { get; set; }
	[NotMapped]
	public IEnumerable<GameKind> ExistsInGameTypes  => _existsInGameTypes ??= GameTypes.Select(gameType => gameType.Type);

	public string? PromoTypes { get; set; }
	[NotMapped]
	public IEnumerable<string>? PromoTypeList => PromoTypes?.Split(',');

	[ForeignKey("ParentCardId")]
	public Card? ParentCard { get; set; }
	public long? ParentCardId { get; set; }

	public DateOnly ReleaseDate { get; set; }
	public bool IsReserved { get; set; }
	public bool CanBeFoundInBoosters { get; set; }
	public bool IsDigitalOnly { get; set; }
	public bool IsFullArt { get; set; }
	public bool IsOversized { get; set; }
	public bool IsPromo { get; set; }
	public bool IsReprint { get; set; }
	public bool IsTextless { get; set; }
}
