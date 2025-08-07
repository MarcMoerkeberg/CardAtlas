using CardAtlas.Server.Models.Data.Base;
using CardAtlas.Server.Models.Data.CardRelations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data;

public class PrintFinish : TypeEntity<PrintFinishType>
{
	[InverseProperty("PrintFinish")]
	public ICollection<CardPrintFinish> CardPrintFinishes { get; set; } = new HashSet<CardPrintFinish>();
}

public enum PrintFinishType
{
	NotImplemented = -1,
	Foil = 1,
	NonFoil = 2,
	Etched = 3,
}
