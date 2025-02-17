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
		AssignCurrencyId(seedData);

		builder.HasData(seedData);
	}

	private static void AssignCurrencyId(IEnumerable<Vendor> vendors)
	{
		foreach (Vendor vendor in vendors)
		{
			vendor.CurrencyId = GetCurrencyId(vendor);
		}
	}

	private static int GetCurrencyId(Vendor vendor)
	{
		return vendor.Type switch 
		{ 
			VendorType.TcgPlayer => (int)CurrencyType.Usd, 
			VendorType.CardMarket => (int)CurrencyType.Eur, 
			VendorType.CardHoarder => (int)CurrencyType.Tix, 
			_ => (int)CurrencyType.NotImplemented
		};
	}
}
