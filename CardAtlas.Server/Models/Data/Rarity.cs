using CardAtlas.Server.Models.Data.Base;

namespace CardAtlas.Server.Models.Data;

public class Rarity : TypeEntity<RarityType>
{
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