using CardAtlas.Server.Models.Data.Base;

namespace CardAtlas.Server.Models.Data
{
	public class Legality : TypeEntity<LegalityType>
	{
	}

	public enum LegalityType
	{
		Legal = 1,
		NotLegal = 2,
		Restricted = 3,
		Banned = 4,
		NotImplemented = 0
	}
}
