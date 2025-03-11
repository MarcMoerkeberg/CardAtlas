using CardAtlas.Server.Helpers;
using CardAtlas.Server.Models.Data.Image;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CardAtlas.Server.DAL.EntityConfigurations;

public class ImageSourceConfiguration : IEntityTypeConfiguration<ImageSource>
{
	public void Configure(EntityTypeBuilder<ImageSource> builder)
	{
		IEnumerable<ImageSource> seedData = EntityConfigurationHelper.GetEnumSeedData<ImageSource, ImageSourceType>();

		builder.HasData(seedData);
	}
}
