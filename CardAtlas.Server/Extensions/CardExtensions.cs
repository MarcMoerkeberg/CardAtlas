using CardAtlas.Server.Models.Data;
using CardFace = ScryfallApi.Models.CardFace;

namespace CardAtlas.Server.Extensions;

public static class CardExtensions
{
	/// <summary>
	/// Finds the first <see cref="Card"/> in <paramref name="targetCollection"/> with properties that match the provided <paramref name="cardFace"/>.
	/// </summary>
	/// <returns>The first <see cref="Card"/> with properties that match <paramref name="cardFace"/>. Returns null if none is found.</returns>
	public static Card? FindMatchByName(this IEnumerable<Card> targetCollection, CardFace cardFace)
	{
		return targetCollection.FirstOrDefault(cardFromDb => 
			string.Equals(cardFromDb.Name, cardFace.Name)
		);
	}
}
