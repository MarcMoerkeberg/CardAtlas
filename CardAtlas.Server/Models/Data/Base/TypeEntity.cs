using System.ComponentModel.DataAnnotations;

namespace CardAtlas.Server.Models.Data.Base;

public class TypeEntity<TEnum> where TEnum : struct, Enum
{
	[Key]
	public required int Id { get; set; }

	[MinLength(1)]
	[MaxLength(30)]
	public required string Name { get; set; }

	public TEnum Type => (TEnum)Enum.ToObject(typeof(TEnum), Id);
}