using CardAtlas.Server.Models.Data.Base;
using CardAtlas.Server.Models.Data.CardRelations;

namespace CardAtlas.Server.Models.Data;

public class GameType : TypeEntity<GameKind>
{
	public ICollection<CardGameTypeAvailability> CardGameTypes { get; set; } = new HashSet<CardGameTypeAvailability>();
}

public enum GameKind
{
	NotImplemented = -1,
	Paper = 1,
	Arena = 2,
	Mtgo = 3,
}
