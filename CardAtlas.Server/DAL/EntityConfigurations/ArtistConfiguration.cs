using CardAtlas.Server.Helpers;
using CardAtlas.Server.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CardAtlas.Server.DAL.EntityConfigurations;

public class ArtistConfiguration : IEntityTypeConfiguration<Artist>
{
	public void Configure(EntityTypeBuilder<Artist> builder)
	{
		IEnumerable<Currency> seedData = EntityConfigurationHelper.GetEnumSeedData<Currency, CurrencyType>();

		builder.HasData(
			new Artist
			{
				Id = -1,
				ScryfallId = null,
				Name = "Unknown - Default artist"
			}
		);
	}
}
