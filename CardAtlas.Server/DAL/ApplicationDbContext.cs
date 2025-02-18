using CardAtlas.Server.Models.Data;
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
	public DbSet<Vendor> Vendors { get; set; }
	public DbSet<Currency> Currencies { get; set; }
}
