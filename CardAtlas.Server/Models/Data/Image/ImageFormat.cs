using CardAtlas.Server.Models.Data.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data.Image;

public class ImageFormat : TypeEntity<ImageFormatType>
{
	[InverseProperty("ImageFormat")]
	public ICollection<CardImage> ImageFormats { get; set; } = new HashSet<CardImage>();
}

public enum ImageFormatType
{
	NotImplemented = -1,
	PNG = 1,
	JPG = 2,
}
