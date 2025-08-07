using CardAtlas.Server.Models.Entities;
using CardAtlas.Server.Models.Internal;
using System.Text.Json;

namespace CardAtlas.Server.Mappers;

public static class OutboxMapper
{
	public static OutboxMessage ToEmailMessage(string email)
	{
		OutboxEmail messagePayload = new OutboxEmail
		{
			ToEmail = email,
			Body = "",
			Subject = ""
		};

		return ToEntity(messagePayload, MessageType.EmailConfirmation);
	}



	private static OutboxMessage ToEntity<TPayload>(TPayload payload, MessageType messageType)
	{
		return new OutboxMessage
		{
			Payload = JsonSerializer.Serialize(payload),
			MessageTypeId = (int)messageType,
			CreatedDate = DateTime.UtcNow
		};
	}
}
