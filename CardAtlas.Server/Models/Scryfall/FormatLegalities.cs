using CardAtlas.Server.Models.Scryfall.Types;

namespace CardAtlas.Server.Models.Scryfall;

public class FormatLegalities
{
	public Legality Standard { get; set; }
	public Legality Future { get; set; }
	public Legality Historic { get; set; }
	public Legality Timeless { get; set; }
	public Legality Gladiator { get; set; }
	public Legality Pioneer { get; set; }
	public Legality Explorer { get; set; }
	public Legality Modern { get; set; }
	public Legality Legacy { get; set; }
	public Legality Pauper { get; set; }
	public Legality Vintage { get; set; }
	public Legality Penny { get; set; }
	public Legality Commander { get; set; }
	public Legality Oathbreaker { get; set; }
	public Legality StandardBrawl { get; set; }
	public Legality Brawl { get; set; }
	public Legality Alchemy { get; set; }
	public Legality PauperCommander { get; set; }
	public Legality Duel { get; set; }
	public Legality Oldschool { get; set; }
	public Legality Premodern { get; set; }
	public Legality Predh { get; set; }
}
