using CardAtlas.Server.Models.Data.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data.Image;

public class ImageFormat : TypeEntity<ImageFormatType>
{
	[InverseProperty("ImageFormat")]
	public required ICollection<CardImage> ImageFormats { get; set; }
}

public enum ImageFormatType
{
	PNG = 1,
	JPG = 2,
	NotImplemented = 0
}
