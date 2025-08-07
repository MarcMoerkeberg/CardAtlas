
using CardAtlas.Server.Models.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardAtlas.Server.Models.Data;

public class Language : TypeEntity<LanguageType>
{
	[MinLength(1)]
	[MaxLength(3)]
	public required string Code { get; set; }

	[MinLength(1)]
	[MaxLength(2)]
	public string? PrintCode { get; set; }

	[InverseProperty("Language")]
	public required ICollection<Card> Cards { get; set; }
}

public enum LanguageType
{
	NotImplemented = -1,
	English = 1,
	Spanish = 2,
	French = 3,
	German = 4,
	Italian = 5,
	Portuguese = 6,
	Japanese = 7,
	Korean = 8,
	Russian = 9,
	SimplifiedChinese = 10,
	TraditionalChinese = 11,
	Hebrew = 12,
	Latin = 13,
	AncientGreek = 14,
	Arabic = 15,
	Sanskrit = 16,
	Phyrexian = 17,
}
