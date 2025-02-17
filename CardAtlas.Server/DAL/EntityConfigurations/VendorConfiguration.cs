using CardAtlas.Server.Helpers;
using CardAtlas.Server.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CardAtlas.Server.DAL.EntityConfigurations;

public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
{
	public void Configure(EntityTypeBuilder<Vendor> builder)
	{
		IEnumerable<Vendor> seedData = EntityConfigurationHelper.GetEnumSeedData<Vendor, VendorType>();
		
		builder.HasData(seedData);
	}
}
