using CardAtlas.Server.Models.Data.Base;

namespace CardAtlas.Server.Models.Data;

public class PrintFinish : TypeEntity<PrintFinishType>
{
	public ICollection<Card> Cards { get; set; } = new HashSet<Card>();
}

public enum PrintFinishType
{
	NotImplemented = -1,
	Foil = 1,
	NonFoil = 2,
	Etched = 3,
}
