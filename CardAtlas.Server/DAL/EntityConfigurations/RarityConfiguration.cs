using CardAtlas.Server.Helpers;
using CardAtlas.Server.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CardAtlas.Server.DAL.EntityConfigurations;

public class RarityConfiguration : IEntityTypeConfiguration<Rarity>
{
	public void Configure(EntityTypeBuilder<Rarity> builder)
	{
		IEnumerable<Rarity> seedData = EntityConfigurationHelper.GetEnumSeedData<Rarity, RarityType>();
		
		builder.HasData(seedData);
	}
}
