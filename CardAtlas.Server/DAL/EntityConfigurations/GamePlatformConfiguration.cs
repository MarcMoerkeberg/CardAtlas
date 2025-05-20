using CardAtlas.Server.Helpers;
using CardAtlas.Server.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CardAtlas.Server.DAL.EntityConfigurations;

public class GamePlatformConfiguration : IEntityTypeConfiguration<GamePlatform>
{
	public void Configure(EntityTypeBuilder<GamePlatform> builder)
	{
		IEnumerable<GamePlatform> seedData = EntityConfigurationHelper.GetEnumSeedData<GamePlatform, PlatformType>();

		builder.HasData(seedData);
	}
}
