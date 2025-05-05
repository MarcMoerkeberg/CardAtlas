using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Data.Image;
using ScryfallApi.Models;
using ApiCard = ScryfallApi.Models.Card;
using CardFace = ScryfallApi.Models.CardFace;
using ImageStatus = ScryfallApi.Models.Types.ImageStatus;

namespace CardAtlas.Server.Mappers;

public class CardImageMapper
{
	/// <summary>
	/// Maps the card images from the Scryfall API to the database model.<br/>
	/// Uses the data from the <paramref name="cardFace"/> if it's not null, otherwise it uses <paramref name="apiCard"/>.
	/// </summary>
	/// <returns>A new list of <see cref="CardImage"/>. May be empty if <paramref name="apiCard"/> has no imagery data.</returns>
	public static List<CardImage> MapCardImages(ApiCard apiCard, CardFace? cardFace = null)
	{
		var cardImages = new List<CardImage>();
		ImageUris? imageUris = cardFace is null
			? apiCard.ImageUris
			: cardFace.ImageUris;
		if (imageUris is null || apiCard.ImageStatus is ImageStatus.Missing) return cardImages;

		ImageStatusType cardImageStatus = GetImageStatusType(apiCard);
		var imageMapping = new (ImageTypeKind Type, Uri? Uri)[]
		{
			(ImageTypeKind.Png, imageUris.Png),
			(ImageTypeKind.BorderCrop, imageUris.BorderCrop),
			(ImageTypeKind.ArtCrop, imageUris.ArtCrop),
			(ImageTypeKind.Large, imageUris.Large),
			(ImageTypeKind.Normal, imageUris.Normal),
			(ImageTypeKind.Small,imageUris.Small)
		};

		foreach (var (imageType, imageUri) in imageMapping)
		{
			if (imageUri is null) continue;

			cardImages.Add(new CardImage
			{
				ImageTypeId = (int)imageType,
				ImageFormatId = (int)GetImageFormatType(imageType),
				ImageStatusId = (int)cardImageStatus,
				CardId = default,
				Width = GetImageWidth(imageType),
				Height = GetImageHeight(imageType),
				Uri = imageUri,
				SourceId = (int)SourceType.Scryfall,
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

	/// <summary>
	/// Assigns all intrinsic properties from the <paramref name="source"/> onto the <paramref name="target"/>.<br/>
	/// These properties represent the core data of the <see cref="CardImage"/> (such as foreign keys, Uri, numeric values, etc.)
	/// that are directly managed by the CardImage entity, excluding any navigational or derived properties.
	/// </summary>
	/// <param name="target">The entity being updated.</param>
	/// <param name="source">The properties of this object will be assigned to <paramref name="target"/>.</param>
	public static void MergeProperties(CardImage target, CardImage source)
	{
		target.Id = source.Id;
		target.ImageTypeId = source.ImageTypeId;
		target.ImageFormatId = source.ImageFormatId;
		target.ImageStatusId = source.ImageStatusId;
		target.SourceId = source.SourceId;
		target.CardId = source.CardId;
		target.Width = source.Width;
		target.Height = source.Height;
		target.Uri = source.Uri;
	}

}
