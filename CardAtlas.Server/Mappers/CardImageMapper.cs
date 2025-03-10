using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.Image;
using ApiCard = ScryfallApi.Models.Card;
using ImageStatus = ScryfallApi.Models.Types.ImageStatus;

namespace CardAtlas.Server.Mappers;

public class CardImageMapper
{
	/// <summary>
	/// Maps the card images from the Scryfall API to the database model.
	/// </summary>
	/// <param name="cardId">The id of the <see cref="Card"/> that owns these images.</param>
	/// <returns>A new list of <see cref="CardImage"/>. May be empty if <paramref name="apiCard"/> has no imagery data.</returns>
	public static IEnumerable<CardImage> MapCardImages(long cardId, ApiCard apiCard)
	{
		var cardImages = new List<CardImage>();
		if (apiCard.ImageUris is null || apiCard.ImageStatus is ImageStatus.Missing) return cardImages;

		ImageStatusType cardImageStatus = GetImageStatusType(apiCard);
		var imageMapping = new (ImageTypeKind Type, Uri? Uri)[]
		{
			(ImageTypeKind.Png, apiCard.ImageUris.Png),
			(ImageTypeKind.BorderCrop, apiCard.ImageUris.BorderCrop),
			(ImageTypeKind.ArtCrop, apiCard.ImageUris.ArtCrop),
			(ImageTypeKind.Large, apiCard.ImageUris.Large),
			(ImageTypeKind.Normal, apiCard.ImageUris.Normal),
			(ImageTypeKind.Small, apiCard.ImageUris.Small)
		};

		foreach (var (imageType, imageUri) in imageMapping)
		{
			if (imageUri is null) continue;

			cardImages.Add(new CardImage
			{
				ImageTypeId = (int)imageType,
				ImageFormatId = (int)GetImageFormatType(imageType),
				ImageStatusId = (int)cardImageStatus,
				CardId = cardId,
				Width = GetImageWidth(imageType),
				Height = GetImageHeight(imageType),
				Uri = imageUri
			});
		}

		return cardImages;
	}

	private static int GetImageWidth(ImageTypeKind imageUriType)
	{
		return imageUriType switch
		{
			ImageTypeKind.Png => 745,
			ImageTypeKind.BorderCrop => 480,
			ImageTypeKind.ArtCrop => 0, //Varies, thus cannot determine from ImageTypeKind
			ImageTypeKind.Large => 672,
			ImageTypeKind.Normal => 488,
			ImageTypeKind.Small => 146,
			_ => 0,
		};
	}

	private static int GetImageHeight(ImageTypeKind imageUriType)
	{
		return imageUriType switch
		{
			ImageTypeKind.Png => 1040,
			ImageTypeKind.BorderCrop => 680,
			ImageTypeKind.ArtCrop => 0, //Varies, thus cannot determine from ImageTypeKind
			ImageTypeKind.Large => 936,
			ImageTypeKind.Normal => 680,
			ImageTypeKind.Small => 204,
			_ => 0,
		};
	}

	private static ImageStatusType GetImageStatusType(ApiCard apiCard)
	{
		return apiCard.ImageStatus switch
		{
			ImageStatus.Placeholder => ImageStatusType.Placeholder,
			ImageStatus.LowRes => ImageStatusType.LowResolution,
			ImageStatus.HighResScan => ImageStatusType.HighResolutionScan,
			_ => ImageStatusType.NotImplemented,
		};
	}

	private static ImageFormatType GetImageFormatType(ImageTypeKind imageUriType)
	{
		return imageUriType switch
		{
			ImageTypeKind.Png => ImageFormatType.Png,
			ImageTypeKind.BorderCrop => ImageFormatType.Jpg,
			ImageTypeKind.ArtCrop => ImageFormatType.Jpg,
			ImageTypeKind.Large => ImageFormatType.Jpg,
			ImageTypeKind.Normal => ImageFormatType.Jpg,
			ImageTypeKind.Small => ImageFormatType.Jpg,
			_ => ImageFormatType.NotImplemented,
		};
	}
}
