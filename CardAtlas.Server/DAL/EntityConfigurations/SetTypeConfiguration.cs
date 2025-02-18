using CardAtlas.Server.Helpers;
using CardAtlas.Server.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CardAtlas.Server.DAL.EntityConfigurations;

public class SetTypeConfiguration : IEntityTypeConfiguration<SetType>
{
	public void Configure(EntityTypeBuilder<SetType> builder)
	{
		IEnumerable<SetType> seedData = EntityConfigurationHelper.GetEnumSeedData<SetType, SetTypeKind>();

		builder.HasData(seedData);
	}
}
