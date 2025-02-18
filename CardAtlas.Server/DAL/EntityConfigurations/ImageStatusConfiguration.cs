using CardAtlas.Server.Helpers;
using CardAtlas.Server.Models.Data.Image;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CardAtlas.Server.DAL.EntityConfigurations;

public class ImageStatusConfiguration : IEntityTypeConfiguration<ImageStatus>
{
	public void Configure(EntityTypeBuilder<ImageStatus> builder)
	{
		IEnumerable<ImageStatus> seedData = EntityConfigurationHelper.GetEnumSeedData<ImageStatus, ImageStatusType>();

		builder.HasData(seedData);
	}
}