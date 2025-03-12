using CardAtlas.Server.Models.Data.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data.Image;

public class ImageSource : TypeEntity<ImageSourceType>
{
	[InverseProperty("ImageSource")]
	public required ICollection<CardImage> CardImages { get; set; }
}

public enum ImageSourceType
{
	NotImplemented = -1,
	Scryfall = 1,
	User = 2,
}
