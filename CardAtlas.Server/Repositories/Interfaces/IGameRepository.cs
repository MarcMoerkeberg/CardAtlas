﻿using CardAtlas.Server.Models.Data;
using CardAtlas.Server.Models.Internal;

namespace CardAtlas.Server.Repositories.Interfaces;

public interface IGameRepository
{
	/// <summary>
	/// Returns all <see cref="GameFormat"/> entities from the database.
	/// </summary>
	Task<IEnumerable<GameFormat>> GetFormats();

	/// <summary>
	/// Returns all <see cref="GameFormat"/> entities from the database with the provided <paramref name="source"/>.
	/// </summary>
	/// <param name="source"></param>
	/// <returns></returns>
	Task<List<GameFormat>> GetFormats(SourceType source);

	/// <summary>
	/// Returns the <see cref="GameFormat"/> entity with the provided <paramref name="formatId"/>.
	/// Throws an <see cref="InvalidOperationException"/> if none, or more than one <see cref="GameFormat"/> entities is found.
	/// </summary>
	/// <exception cref="InvalidOperationException">Thrown if none, or more than one <see cref="GameFormat"/> entities is found.</exception>
	Task<GameFormat> GetFormat(int formatId);

	/// <summary>
	/// Creates the provided <paramref name="format"/> to the database.
	/// </summary>
	/// <returns>The added <see cref="GameFormat"/> with identity.</returns>
	Task<GameFormat> Create(GameFormat format);

	/// <summary>
	/// Creates a new <see cref="GameFormat"/> entry in the database for each <paramref name="formats"/>.
	/// </summary>
	/// <returns>The number of added <see cref="GameFormat"/> entities.</returns>
	Task<int> Create(IEnumerable<GameFormat> formats);

	/// <summary>
	/// Creates and updates <see cref="GameFormat"/> entities, based on the provided <paramref name="upsertionData"/>.
	/// </summary>
	/// <returns>The total number of inserted or updated <see cref="GameFormat"/> entities.</returns>
	Task<int> Upsert(UpsertContainer<GameFormat> upsertionData);
}