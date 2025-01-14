namespace CardAtlas.Server.Models.Data;

public class Card
{
	public int Id { get; set; }
	public required string Name { get; set; }
	public string? Description { get; set; }
}
