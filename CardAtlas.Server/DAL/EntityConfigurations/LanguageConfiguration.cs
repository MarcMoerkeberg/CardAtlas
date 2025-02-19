using CardAtlas.Server.Helpers;
using CardAtlas.Server.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CardAtlas.Server.DAL.EntityConfigurations;

public class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
	public void Configure(EntityTypeBuilder<Language> builder)
	{
		IEnumerable<Language> seedData = EntityConfigurationHelper.GetEnumSeedData<Language, LanguageType>();
		AssignLanguageCodes(seedData);

		builder.HasData(seedData);
	}

	private static void AssignLanguageCodes(IEnumerable<Language> languages)
	{
		foreach (Language language in languages)
		{
			language.Code = GetLanguageCode(language);
			language.PrintCode = GetLanguagePrintCode(language);
		}
	}

	private static string GetLanguageCode(Language language)
	{
		return language.Type switch 
		{
			LanguageType.English => "en",
			LanguageType.Spanish => "es",
			LanguageType.French => "fr",
			LanguageType.German => "de",
			LanguageType.Italian => "it",
			LanguageType.Portuguese => "pt",
			LanguageType.Japanese => "ja",
			LanguageType.Korean => "ko",
			LanguageType.Russian => "ru",
			LanguageType.SimplifiedChinese => "zhs",
			LanguageType.TraditionalChinese => "zht",
			LanguageType.Hebrew => "he",
			LanguageType.Latin => "la",
			LanguageType.AncientGreek => "grc",
			LanguageType.Arabic => "ar",
			LanguageType.Sanskrit => "sa",
			LanguageType.Phyrexian => "ph",
			_ => "NA"
		};
	}
	
	private static string? GetLanguagePrintCode(Language language)
	{
		return language.Type switch 
		{
			LanguageType.English => "en",
			LanguageType.Spanish => "sp",
			LanguageType.French => "fr",
			LanguageType.German => "de",
			LanguageType.Italian => "it",
			LanguageType.Portuguese => "pt",
			LanguageType.Japanese => "jp",
			LanguageType.Korean => "kr",
			LanguageType.Russian => "ru",
			LanguageType.SimplifiedChinese => "cs",
			LanguageType.TraditionalChinese => "ct",
			LanguageType.Phyrexian => "ph",
			_ => null
		};
	}
}
