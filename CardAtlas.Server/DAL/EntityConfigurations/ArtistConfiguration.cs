using CardAtlas.Server.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CardAtlas.Server.DAL.EntityConfigurations;

public class ArtistConfiguration : IEntityTypeConfiguration<Artist>
{
	public void Configure(EntityTypeBuilder<Artist> builder)
	{
		Artist defaultArtist = new Artist
		{
			Id = Artist.DefaultId,
			Name = "Unknown - Default artist"
		};

		builder.HasData(defaultArtist);
	}
}
