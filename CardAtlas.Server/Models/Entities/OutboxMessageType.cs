using CardAtlas.Server.Models.Data.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Entities;

public class OutboxMessageType : TypeEntity<MessageType>
{
	[InverseProperty("MessageType")]
	public ICollection<OutboxMessage> OutboxMessages { get; set; } = new HashSet<OutboxMessage>();
}

public enum MessageType
{
	NotImplemented = -1,
	EmailConfirmation = 1,
}