using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Entities;

public class OutboxMessage
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public Guid Id { get; init; }

	[ForeignKey(nameof(MessageTypeId))]
	public OutboxMessageType MessageType { get; set; } = null!;
	public required int MessageTypeId { get; init; }

	[MinLength(1)]
	[MaxLength(1024)]
	public required string Payload { get; init; }
	public bool IsProcessed { get; set; }
	public int RetryCount { get; set; }
	public required DateTime CreatedDate { get; set; }
	public DateTime? LastModifiedDate { get; set; }
}
