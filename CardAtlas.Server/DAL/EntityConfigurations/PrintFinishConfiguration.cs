using CardAtlas.Server.Helpers;
using CardAtlas.Server.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CardAtlas.Server.DAL.EntityConfigurations;

public class PrintFinishConfiguration : IEntityTypeConfiguration<PrintFinish>
{
	public void Configure(EntityTypeBuilder<PrintFinish> builder)
	{
		IEnumerable<PrintFinish> seedData = EntityConfigurationHelper.GetEnumSeedData<PrintFinish, PrintFinishType>();

		builder.HasData(seedData);
	}
}
