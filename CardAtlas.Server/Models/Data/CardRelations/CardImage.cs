using CardAtlas.Server.Models.Data.Image;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data.Cards;

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

	[ForeignKey("CardId")]
	public Card Card { get; set; } = null!;
	public required long CardId { get; set; }

	public required int Width { get; set; }
	public required int Height { get; set; }
	public required Uri Uri { get; set; }
}