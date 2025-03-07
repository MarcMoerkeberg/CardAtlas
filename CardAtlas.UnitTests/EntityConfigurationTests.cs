using CardAtlas.Server.Helpers;
using CardAtlas.Server.Models.Data;

namespace CardAtlas.UnitTests
{
	public class EntityConfigurationTests
	{
		[Test]
		public void GetEnumSeedData_CanCreateSeedData_ExpectsEntityTypePropertiesToBePopulated()
		{
			IEnumerable<Rarity> seedData = EntityConfigurationHelper.GetEnumSeedData<Rarity, RarityType>();

			Assert.That(seedData, Is.Not.Null);
			Assert.That(seedData, Is.Not.Empty);
			Assert.That(seedData, Has.All.Property(nameof(Rarity.Id)).Not.Null);
			Assert.That(seedData, Has.All.Property(nameof(Rarity.Name)).Not.Null);
		}
	}
}