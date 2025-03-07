using CardAtlas.Server.Models.Data;
using ApiSet = ScryfallApi.Models.Set;
using ScryfallSetType = ScryfallApi.Models.Types.SetType;

namespace CardAtlas.Server.Mappers;

public static class SetMapper
{
	/// <summary>
	/// Maps the set properties on each <paramref name="apiSets"/> to a new <see cref="Set"/>.
	/// </summary>
	public static IEnumerable<Set> MapSets(IEnumerable<ApiSet> apiSets)
	{
		return apiSets.Select(MapSet);
	}

	/// <summary>
	/// Maps the set properties on <paramref name="apiSet"/> to a new <see cref="Set"/>.
	/// </summary>
	public static Set MapSet(ApiSet apiSet)
	{
		return new Set
		{
			ScryfallId = apiSet.Id,
			Name = apiSet.Name,
			Code = apiSet.SetCode,
			MtgoCode = apiSet.MtgoSetCode,
			ArenaCode = apiSet.ArenaSetCode,
			ParentSetCode = apiSet.ParentSetCode,
			Block = apiSet.Block,
			BlockCode = apiSet.BlockCode,
			SetTypeId = (int)GetSetType(apiSet),
			NumberOfCardsInSet = apiSet.CardCountInSet,
			IsDigitalOnly = apiSet.IsDigitalOnly,
			IsFoilOnly = apiSet.IsFoilOnly,
			IsNonFoilOnly = apiSet.IsNonFoilOnly,
			ReleaseDate = apiSet.ReleasedDate,
		};
	}

	private static SetTypeKind GetSetType(ApiSet apiSet)
	{
		return apiSet.SetType switch
		{
			ScryfallSetType.Core => SetTypeKind.Core,
			ScryfallSetType.Expansion => SetTypeKind.Expansion,
			ScryfallSetType.Masters => SetTypeKind.Masters,
			ScryfallSetType.Alchemy => SetTypeKind.Alchemy,
			ScryfallSetType.Masterpiece => SetTypeKind.Masterpiece,
			ScryfallSetType.Arsenal => SetTypeKind.Arsenal,
			ScryfallSetType.FromTheVault => SetTypeKind.FromTheVault,
			ScryfallSetType.Spellbook => SetTypeKind.Spellbook,
			ScryfallSetType.PremiumDeck => SetTypeKind.PremiumDeck,
			ScryfallSetType.DuelDeck => SetTypeKind.DuelDeck,
			ScryfallSetType.DraftInnovation => SetTypeKind.DraftInnovation,
			ScryfallSetType.TreasureChest => SetTypeKind.TreasureChest,
			ScryfallSetType.Commander => SetTypeKind.Commander,
			ScryfallSetType.Planechase => SetTypeKind.Planechase,
			ScryfallSetType.Archenemy => SetTypeKind.Archenemy,
			ScryfallSetType.Vanguard => SetTypeKind.Vanguard,
			ScryfallSetType.Funny => SetTypeKind.Funny,
			ScryfallSetType.Starter => SetTypeKind.Starter,
			ScryfallSetType.Box => SetTypeKind.Box,
			ScryfallSetType.Promo => SetTypeKind.Promo,
			ScryfallSetType.Token => SetTypeKind.Token,
			ScryfallSetType.Memorabilia => SetTypeKind.Memorabilia,
			ScryfallSetType.MiniGame => SetTypeKind.MiniGame,
			_ => SetTypeKind.NotImplemented,
		};
	}
}
