using CardAtlas.Server.Models.Data.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data;

public class FrameLayout : TypeEntity<FrameType>
{
	[InverseProperty("FrameLayout")]
	public ICollection<Card> Cards { get; set; } = new HashSet<Card>();
}

public enum FrameType
{
	NotImplemented = -1,
	Year1993 = 1,
	Year1997 = 2,
	Year2003 = 3,
	Year2015 = 4,
	Future = 5,
}