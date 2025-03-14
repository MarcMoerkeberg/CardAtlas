using CardAtlas.Server.Helpers;
using CardAtlas.Server.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CardAtlas.Server.DAL.EntityConfigurations;

public class ImageSourceConfiguration : IEntityTypeConfiguration<Source>
{
	public void Configure(EntityTypeBuilder<Source> builder)
	{
		IEnumerable<Source> seedData = EntityConfigurationHelper.GetEnumSeedData<Source, SourceType>();

		builder.HasData(seedData);
	}
}
