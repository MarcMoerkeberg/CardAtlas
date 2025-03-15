using CardAtlas.Server.Models.Data.Base;
using CardAtlas.Server.Models.Data.CardRelations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data
{
	public class Legality : TypeEntity<LegalityType>
	{
		[InverseProperty("Legality")]
		public ICollection<CardLegality> CardLegalities { get; set; } = new HashSet<CardLegality>();
	}

	public enum LegalityType
	{
		NotImplemented = -1,
		Legal = 1,
		NotLegal = 2,
		Restricted = 3,
		Banned = 4,
	}
}
