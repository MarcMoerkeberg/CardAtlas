using CardAtlas.Server.Models.Data.Base;
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
	Placeholder = 1,
	LowResolution = 2,
	HighResolutionScan = 3,
}
