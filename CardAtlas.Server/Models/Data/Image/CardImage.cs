using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data.Image;

public class CardImage
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long Id { get; set; }

	[ForeignKey("ImageTypeId")]
	public required ImageType ImageType { get; set; }
	public int ImageTypeId { get; set; }
	[NotMapped]
	public ImageTypeKind Type => ImageType.Type;

	[ForeignKey("ImageFormatId")]
	public required ImageFormat ImageFormat { get; set; }
	public int ImageFormatId { get; set; }
	[NotMapped]
	public ImageFormatType Format => ImageFormat.Type;

	[ForeignKey("ImageStatusId")]
	public required ImageStatus ImageStatus { get; set; }
	public int ImageStatusId { get; set; }
	[NotMapped]
	public ImageStatusType Status => ImageStatus.Type;

	[ForeignKey("CardId")]
	public required Card Card { get; set; }
	public long CardId { get; set; }

	public int Width { get; set; }
	public int Height { get; set; }
	public required Uri Uri { get; set; }
}