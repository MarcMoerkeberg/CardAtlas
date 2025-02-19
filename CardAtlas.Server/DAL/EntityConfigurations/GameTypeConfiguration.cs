using CardAtlas.Server.Helpers;
using CardAtlas.Server.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CardAtlas.Server.DAL.EntityConfigurations;

public class GameTypeConfiguration : IEntityTypeConfiguration<GameType>
{
	public void Configure(EntityTypeBuilder<GameType> builder)
	{
		IEnumerable<GameType> seedData = EntityConfigurationHelper.GetEnumSeedData<GameType, GameKind>();

		builder.HasData(seedData);
	}
}
