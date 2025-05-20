using CardAtlas.Server.Models.Data.Base;
using CardAtlas.Server.Models.Data.CardRelations;

namespace CardAtlas.Server.Models.Data;

public class GamePlatform : TypeEntity<PlatformType>
{
	public ICollection<CardGamePlatform> CardGameTypes { get; set; } = new HashSet<CardGamePlatform>();
}

public enum PlatformType
{
	NotImplemented = -1,
	Paper = 1,
	Arena = 2,
	Mtgo = 3,
}
