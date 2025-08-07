using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data.Base;

public class TypeEntity<TEnum> where TEnum : struct, Enum
{
	[Key]
	public required int Id { get; set; }

	[MinLength(1)]
	[MaxLength(30)]
	public required string Name { get; set; }

	[NotMapped]
	private TEnum? _type { get; set; }
	[NotMapped]
	public TEnum Type => _type ??= (TEnum)Enum.ToObject(typeof(TEnum), Id);
}