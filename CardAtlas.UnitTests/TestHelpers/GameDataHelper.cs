using CardAtlas.Server.Models.Data;

namespace CardAtlas.UnitTests.DataHelpers;

public static class GameDataHelper
{
	public static GameFormat CreateGameFormat(string formatName = "Gameformat", SourceType source = SourceType.NotImplemented)
	{
		return new GameFormat
		{
			Id = 0,
			Name = formatName,
			SourceId = (int)source,
			Description = "Description of the game format."
		};
	}
}
