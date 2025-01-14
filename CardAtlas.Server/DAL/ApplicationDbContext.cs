using CardAtlas.Server.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace CardAtlas.Server.DAL;

public class ApplicationDbContext : DbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
	public DbSet<Card> Cards { get; set; }
}
