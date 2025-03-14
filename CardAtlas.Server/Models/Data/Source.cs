using CardAtlas.Server.Models.Data.Base;
using CardAtlas.Server.Models.Data.Image;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data;

public class Source : TypeEntity<SourceType>
{
	[InverseProperty("ImageSource")]
	public required ICollection<CardImage> CardImages { get; set; }
}

public enum SourceType
{
	NotImplemented = -1,
	Scryfall = 1,
	User = 2,
}
