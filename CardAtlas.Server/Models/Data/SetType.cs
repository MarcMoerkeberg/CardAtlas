using CardAtlas.Server.Models.Data.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data;

public class SetType : TypeEntity<SetKind>
{
	[InverseProperty("SetType")]
	public required ICollection<Set> Sets { get; set; }
}

public enum SetKind
{
	Core = 1,
	Expansion = 2,
	Masters = 3,
	Alchemy = 4,
	Masterpiece = 5,
	Arsenal = 6,
	FromTheVault = 7,
	Spellbook = 8,
	PremiumDeck = 9,
	DuelDeck = 10,
	DraftInnovation = 11,
	TreasureChest = 12,
	Commander = 13,
	Planechase = 14,
	Archenemy = 15,
	Vanguard = 16,
	Funny = 17,
	Starter = 18,
	Box = 19,
	Promo = 20,
	Token = 21,
	Memorabilia = 22,
	MiniGame = 23,
	NotImplemented = 0
}