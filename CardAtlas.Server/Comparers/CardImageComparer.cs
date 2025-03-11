using CardAtlas.Server.Models.Data.Image;

namespace CardAtlas.Server.Comparers;

public class CardImageComparer : IEqualityComparer<CardImage>
{
	public bool Equals(CardImage? x, CardImage? y)
	{
		if (x is null && y is null) return true;
		if (x is null || y is null) return false;

		return x.Id == y.Id
			&& x.ImageTypeId == y.ImageTypeId
			&& x.ImageFormatId == y.ImageFormatId
			&& x.ImageStatusId == y.ImageStatusId
			&& x.CardId == y.CardId
			&& x.Width == y.Width
			&& x.Height == y.Height
			&& x.Uri == y.Uri
			&& x.ImageSourceId == y.ImageSourceId;
	}

	public int GetHashCode(CardImage obj)
	{
		var hash = new HashCode();
		hash.Add(obj.Id);
		hash.Add(obj.ImageTypeId);
		hash.Add(obj.ImageFormatId);
		hash.Add(obj.ImageStatusId);
		hash.Add(obj.CardId);
		hash.Add(obj.Width);
		hash.Add(obj.Height);
		hash.Add(obj.Uri);
		hash.Add(obj.ImageSourceId);

		return hash.ToHashCode();
	}
}