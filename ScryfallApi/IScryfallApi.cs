﻿using ScryfallApi.Models;
using ScryfallApi.Models.Types;

namespace ScryfallApi;

public interface IScryfallApi
{
	/// <summary>
	/// Gets bulk card data from the Scryfall API. <br/>
	/// <b>WARNING:</b> This data is materialized into memory and may be a very large dataset. Use this method cautiously. <br/><br/>
	/// Throws an exception if the supplied <paramref name="dataType"/> does not correspond to a bulk data object that returns <see cref="Card"/> objects.<br/>
	/// See <see href="https://scryfall.com/docs/api/bulk-data">Scryfall API documentation</see> for more information.
	/// </summary>
	/// <param name="dataType">The type of bulk card data fetched.</param>
	/// <returns>An <see cref="IEnumerable{Card}"/> populated with <see cref="Card"/> objects.</returns>
	/// <exception cref="ArgumentException"></exception>
	/// <exception cref="HttpRequestException"></exception>
	/// <exception cref="Exception"></exception>
	public Task<IEnumerable<Card>> GetBulkCardData(BulkDataType dataType);

	/// <summary>
	/// Gets bulk card data from the Scryfall API and returns the data asynchronously as it is deserialized. <br/><br/>
	/// Throws an exception if the supplied <paramref name="dataType"/> does not correspond to a bulk data object that returns <see cref="Card"/> objects.<br/>
	/// See <see href="https://scryfall.com/docs/api/bulk-data">Scryfall API documentation</see> for more information.
	/// </summary>
	/// <param name="dataType">The type of bulk card data fetched.</param>
	/// <returns><see cref="Card"/> asynchronously as they are deserialized.</returns>
	/// <exception cref="ArgumentException"></exception>
	/// <exception cref="HttpRequestException"></exception>
	/// <exception cref="Exception"></exception>
	public IAsyncEnumerable<Card> GetBulkCardDataAsync(BulkDataType dataType);

	/// <summary>
	/// Gets bulk card rulings from the Scryfall API. <br/>
	/// <b>WARNING:</b> This data is materialized into memory and may be a very large dataset. Use this method cautiously. <br/><br/>
	/// </summary>
	/// <returns>An <see cref="IEnumerable{Ruling}"/> populated with <see cref="Ruling"/> objects.</returns>
	/// <exception cref="HttpRequestException"></exception>
	/// <exception cref="Exception"></exception>
	public Task<IEnumerable<Ruling>> GetBulkCardRulings();

	/// <summary>
	/// Gets bulk card rulings from the Scryfall API and returns the data asynchronously as it is deserialized.
	/// </summary>
	/// <returns><see cref="Ruling"/> asynchronously as they are deserialized.</returns>
	/// <exception cref="HttpRequestException"></exception>
	/// <exception cref="Exception"></exception>
	public IAsyncEnumerable<Ruling> GetBulkCardRulingsAsync();

	/// <summary>
	/// Gets all set objects from the Scryfall API. <br/>
	/// See <see href="https://scryfall.com/docs/api/sets/all">Scryfall API documentation</see> for more information.<br/>
	/// </summary>
	/// <returns>An <see cref="IEnumerable{Set}"/> populated with <see cref="Set"/> objects.</returns>
	/// <exception cref="HttpRequestException"></exception>
	/// <exception cref="Exception"></exception>
	public Task<IEnumerable<Set>> GetSets();

	/// <summary>
	/// Gets a single set object from the Scryfall API. <br/>
	/// See <see href="https://scryfall.com/docs/api/sets/id">Scryfall API documentation</see> for more information.<br/>
	/// </summary>
	/// <param name="id">Scryfalls id for the set.</param>
	/// <returns>The <see cref="Set"/> with <paramref name="id"/> as it's id.</returns>
	/// <exception cref="HttpRequestException"></exception>
	/// <exception cref="Exception"></exception>
	public Task<Set> GetSet(Guid id);

	/// <summary>
	/// Gets all CardSymbol objects from the Scryfall API. <br/>
	/// See <see href="https://scryfall.com/docs/api/card-symbol/all">Scryfall API documentation</see> for more information.<br/>
	/// </summary>
	/// <returns>An <see cref="IEnumerable{CardSymbol}"/> populated with <see cref="CardSymbol"/> objects.</returns>
	/// <exception cref="HttpRequestException"></exception>
	/// <exception cref="Exception"></exception>
	public Task<IEnumerable<CardSymbol>> GetCardSymbols();
}
