﻿using CardAtlas.Server.Models.Data.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data;

public class Rarity : TypeEntity<RarityType>
{
	[InverseProperty("Rarity")]
	public ICollection<Card> Cards { get; set; } = new HashSet<Card>();
}

public enum RarityType
{
	NotImplemented = -1,
	Common = 1,
	Uncommon = 2,
	Rare = 3,
	Special = 4,
	Mythic = 5,
	Bonus = 6,
}