using CardAtlas.Server.Models.Data.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data;

public class Rarity : TypeEntity<RarityType>
{
	[InverseProperty("Rarity")]
	public required ICollection<Card> Cards { get; set; }
}

public enum RarityType
{
	Common = 1,
	Uncommon = 2,
	Rare = 3,
	Special = 4,
	Mythic = 5,
	Bonus = 6,
	NotImplemented = 0
}