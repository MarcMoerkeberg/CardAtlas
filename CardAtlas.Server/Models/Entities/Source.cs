using CardAtlas.Server.Models.Data.Base;

namespace CardAtlas.Server.Models.Data;

public class Source : TypeEntity<SourceType>
{
}

public enum SourceType
{
	NotImplemented = -1,
	Scryfall = 1,
	User = 2,
}
