using CardAtlas.Server.Models.Data.Base;

namespace CardAtlas.Server.Models.Data;

public class GameType : TypeEntity<GameKind>
{
	public ICollection<Card> Cards { get; set; } = new HashSet<Card>();
}

public enum GameKind
{
	NotImplemented = -1,
	Paper = 1,
	Arena = 2,
	Mtgo = 3,
}
