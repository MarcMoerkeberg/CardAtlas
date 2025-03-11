using CardAtlas.Server.Models.Data.Image;

namespace CardAtlas.Server.Extensions;

public static class CardImageExtensions
{
	/// <summary>
	/// Finds the first <see cref="CardImage"/> in the <paramref name="targetCollection"/> that matches the <paramref name="cardImage"/>.
	/// </summary>
	/// <returns>The first <see cref="CardImage"/> with properties that match <paramref name="cardImage"/>. Returns null if none is found.</returns>
	public static CardImage? FindMatchByTypeAndSource(this IEnumerable<CardImage> targetCollection, CardImage cardImage)
	{
		return targetCollection.FirstOrDefault(collectionImage =>
			collectionImage.Source == cardImage.Source &&
			collectionImage.Type == cardImage.Type &&
			collectionImage.CardId == cardImage.CardId
		);
	}
}
