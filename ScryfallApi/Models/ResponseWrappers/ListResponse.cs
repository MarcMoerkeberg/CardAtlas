﻿using ScryfallApi.Models.Types;
using System.Text.Json.Serialization;

namespace ScryfallApi.Models.ResponseWrappers;

public class ListResponse<TData>
{
	[JsonPropertyName("object")]
	public required string ScryfallObjectType { get; set; }
	[JsonIgnore]
	private ObjectType? _objectType { get; set; }
	[JsonIgnore]
	public ObjectType ObjectType => _objectType ??= ScryfallParser.ToEnumKey<ObjectType>(ScryfallObjectType);

	[JsonPropertyName("data")]
	public required IEnumerable<TData> Data { get; set; }

	[JsonPropertyName("has_more")]
	public bool HasMore { get; set; }

	[JsonPropertyName("next_page")]
	public Uri? NextPage { get; set; }

	[JsonPropertyName("total_cards")]
	public int? TotalCards { get; set; }

	[JsonPropertyName("warnings")]
	public string[]? Warnings { get; set; }
}
