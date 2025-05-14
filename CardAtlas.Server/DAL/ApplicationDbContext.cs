using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.CardRelations;
using CardAtlas.Server.Models.Data.Image;
using Microsoft.EntityFrameworkCore;

namespace CardAtlas.Server.DAL;

public class ApplicationDbContext : DbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
	}

	public DbSet<Card> Cards { get; set; }
	public DbSet<Rarity> Rarities { get; set; }
	public DbSet<Set> Sets { get; set; }
	public DbSet<SetType> SetTypes { get; set; }
	public DbSet<Vendor> Vendors { get; set; }
	public DbSet<Currency> Currencies { get; set; }
	public DbSet<CardPrice> CardPrices { get; set; }
	public DbSet<Artist> Artists { get; set; }
	public DbSet<CardImage> CardImages { get; set; }
	public DbSet<ImageStatus> ImageStatuses { get; set; }
	public DbSet<ImageFormat> ImageFormats { get; set; }
	public DbSet<ImageType> ImageTypes { get; set; }
	public DbSet<Language> Languages { get; set; }
	public DbSet<Keyword> Keywords { get; set; }
	public DbSet<Legality> Legalities { get; set; }
	public DbSet<CardLegality> CardLegalities { get; set; }
	public DbSet<FrameLayout> FrameLayouts { get; set; }
	public DbSet<PrintFinish> PrintFinishes { get; set; }
	public DbSet<GameType> GameTypes { get; set; }
	public DbSet<Source> Sources { get; set; }
	public DbSet<CardPrintFinish> CardPrintFinishes { get; set; }
	public DbSet<CardGameTypeAvailability> CardGameTypeAvailability { get; set; }
	public DbSet<GameFormat> GameFormats { get; set; }
	public DbSet<CardKeyword> CardKeywords { get; set; }
	public DbSet<PromoType> PromoTypes { get; set; }
	public DbSet<CardPromoType> CardPromoTypes { get; set; }
}
