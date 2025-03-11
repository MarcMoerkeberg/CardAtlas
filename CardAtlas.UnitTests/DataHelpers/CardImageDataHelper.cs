using CardAtlas.Server.Models.Data.Image;

namespace CardAtlas.UnitTests.DataHelpers;

public static class CardImageDataHelper
{
	public static CardImage CreateCardImage(
		long cardId = 1,
		ImageSourceType source = ImageSourceType.NotImplemented,
		ImageTypeKind imageType = ImageTypeKind.NotImplemented) => new CardImage
	{
		CardId = cardId,
		ImageFormatId = (int)ImageFormatType.NotImplemented,
		ImageSourceId = (int)source,
		ImageStatusId = (int)ImageStatusType.NotImplemented,
		ImageTypeId = (int)imageType,
		Uri = new Uri("https://testUri.com"),
		Width = 0,
		Height = 0,
	};
}
