using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data.Image;

public class CardImage
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long Id { get; set; }

	[ForeignKey("ImageTypeId")]
	public ImageType ImageType { get; set; } = null!;
	public required int ImageTypeId { get; set; }
	[NotMapped]
	public ImageTypeKind Type => ImageType.Type;

	[ForeignKey("ImageFormatId")]
	public ImageFormat ImageFormat { get; set; } = null!;
	public required int ImageFormatId { get; set; }
	[NotMapped]
	public ImageFormatType Format => ImageFormat.Type;

	[ForeignKey("ImageStatusId")]
	public ImageStatus ImageStatus { get; set; } = null!;
	public required int ImageStatusId { get; set; }
	[NotMapped]
	public ImageStatusType Status => ImageStatus.Type;
	
	[ForeignKey("SourceId")]
	[DeleteBehavior(DeleteBehavior.Restrict)]
	public Source Source { get; set; } = null!;
	public required int SourceId { get; set; }
	[NotMapped]
	public SourceType SourceType => Source.Type;
	//TODO: Add nullable userId to this entity, to ensure that only the user who uploaded the image can delete it.

	[ForeignKey("CardId")]
	public Card Card { get; set; } = null!;
	public required long CardId { get; set; }

	public required int Width { get; set; }
	public required int Height { get; set; }
	public required Uri Uri { get; set; }
}