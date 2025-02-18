using CardAtlas.Server.Models.Data.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data.Image;

public class ImageStatus : TypeEntity<ImageStatusType>
{
	[InverseProperty("ImageStatus")]
	public required ICollection<CardImage> CardImages { get; set; }
}

public enum ImageStatusType
{
	Missing = 1,
	Placeholder = 2,
	LowResolution = 3,
	HighResolutionScan = 4,
	NotImplemented = 0,
}
