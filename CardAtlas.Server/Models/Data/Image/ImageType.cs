using CardAtlas.Server.Models.Data.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data.Image;

public class ImageType : TypeEntity<ImageTypeKind>
{
	[InverseProperty("ImageType")]
	public required ICollection<CardImage> CardImages { get; set; }
}

public enum ImageTypeKind
{
	Png = 1,
	BorderCrop = 2,
	ArtCrop = 3,
	Large = 4,
	Normal = 5,
	Small = 6,
	NotImplemented = 0
}
