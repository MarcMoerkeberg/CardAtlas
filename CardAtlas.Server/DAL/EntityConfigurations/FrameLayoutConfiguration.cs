using CardAtlas.Server.Helpers;
using CardAtlas.Server.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CardAtlas.Server.DAL.EntityConfigurations;

public class FrameLayoutConfiguration : IEntityTypeConfiguration<FrameLayout>
{
	public void Configure(EntityTypeBuilder<FrameLayout> builder)
	{
		IEnumerable<FrameLayout> seedData = EntityConfigurationHelper.GetEnumSeedData<FrameLayout, FrameType>();

		builder.HasData(seedData);
	}
}
