using CardAtlas.Server.Models.Data.Base;
using CardAtlas.Server.Models.Data.Cards;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data.Image;

public class ImageStatus : TypeEntity<ImageStatusType>
{
	[InverseProperty("ImageStatus")]
	public ICollection<CardImage> CardImages { get; set; } = new HashSet<CardImage>();
}

public enum ImageStatusType
{
	NotImplemented = -1,
	Missing = 1,
	Placeholder = 2,
	LowResolution = 3,
	HighResolutionScan = 4,
}
