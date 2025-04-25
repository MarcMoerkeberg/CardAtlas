using CardAtlas.Server.Helpers;
using CardAtlas.Server.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CardAtlas.Server.DAL.EntityConfigurations;

public class LegalityConfiguration : IEntityTypeConfiguration<Legality>
{
	public void Configure(EntityTypeBuilder<Legality> builder)
	{
		IEnumerable<Legality> seedData = EntityConfigurationHelper.GetEnumSeedData<Legality, LegalityType>();

		builder.HasData(seedData);
	}
}