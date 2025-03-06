using CardAtlas.Server.Models.Data.Base;
using CardAtlas.Server.Models.Data.CardRelations;

namespace CardAtlas.Server.Models.Data;

public class GameType : TypeEntity<GameKind>
{
	public ICollection<CardGameType> CardGameTypes { get; set; } = new HashSet<CardGameType>();
}

public enum GameKind
{
	NotImplemented = -1,
	Paper = 1,
	Arena = 2,
	Mtgo = 3,
}
