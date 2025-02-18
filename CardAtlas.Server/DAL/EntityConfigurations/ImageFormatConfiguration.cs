using CardAtlas.Server.Helpers;
using CardAtlas.Server.Models.Data.Image;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CardAtlas.Server.DAL.EntityConfigurations;

public class ImageFormatConfiguration : IEntityTypeConfiguration<ImageFormat>
{
	public void Configure(EntityTypeBuilder<ImageFormat> builder)
	{
		IEnumerable<ImageFormat> seedData = EntityConfigurationHelper.GetEnumSeedData<ImageFormat, ImageFormatType>();

		builder.HasData(seedData);
	}
}
