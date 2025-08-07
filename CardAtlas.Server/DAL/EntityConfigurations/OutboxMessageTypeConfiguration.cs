using CardAtlas.Server.Helpers;
using CardAtlas.Server.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CardAtlas.Server.DAL.EntityConfigurations;

public class OutboxMessageTypeConfiguration : IEntityTypeConfiguration<OutboxMessageType>
{
	public void Configure(EntityTypeBuilder<OutboxMessageType> builder)
	{
		IEnumerable<OutboxMessageType> seedData = EntityConfigurationHelper.GetEnumSeedData<OutboxMessageType, MessageType>();

		builder.HasData(seedData);
	}
}
