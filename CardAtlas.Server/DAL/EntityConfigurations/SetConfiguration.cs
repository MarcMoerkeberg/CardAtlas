using CardAtlas.Server.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CardAtlas.Server.DAL.EntityConfigurations;

public class SetConfiguration : IEntityTypeConfiguration<Set>
{
	public void Configure(EntityTypeBuilder<Set> builder)
	{
		Set defaultSet = new Set
		{
			Id = Set.DefaultId,
			Code = "Defaul",
			SetTypeId = (int)SetTypeKind.NotImplemented,
			SourceId = (int)SourceType.NotImplemented,
			Name = "Unknown - Default set"
		};

		builder.HasData(defaultSet);
	}
}
