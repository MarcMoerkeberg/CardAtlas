using CardAtlas.Server.Helpers;
using CardAtlas.Server.Models.Data.Image;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CardAtlas.Server.DAL.EntityConfigurations;

public class ImageTypeConfiguration : IEntityTypeConfiguration<ImageType>
{
	public void Configure(EntityTypeBuilder<ImageType> builder)
	{
		IEnumerable<ImageType> seedData = EntityConfigurationHelper.GetEnumSeedData<ImageType, ImageTypeKind>();

		builder.HasData(seedData);
	}
}
