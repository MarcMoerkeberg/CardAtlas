using CardAtlas.Server.Models.Data.Base;

namespace CardAtlas.Server.Helpers;

public static class EntityConfigurationHelper
{
	/// <summary>
	/// Generates seed data <typeparamref name="TEntity"/> based on both the numeric ang stringified values of <typeparamref name="TEnum"/>.<br/>
	/// Only populates properties from <see cref="TypeEntity{TEnum}"/>, so beware that some properties on <typeparamref name="TEntity"/> may be null.
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	/// <typeparam name="TEnum"></typeparam>
	/// <returns>An <see cref="IEnumerable{T}"/> of <typeparamref name="TEntity"/> with the properties of <see cref="TypeEntity{TEnum}"/> populated by values of <typeparamref name="TEnum"/>.</returns>
	/// <exception cref="Exception"/>
	public static IEnumerable<TEntity> GetEnumSeedData<TEntity, TEnum>() 
		where TEntity : TypeEntity<TEnum>
		where TEnum : struct, Enum
	{
		try
		{
			var seedData = Enum
				.GetValues<TEnum>()
				.Cast<TEnum>()
				.Select(enumInstance =>
				{
					//Uses reflection to create instance of TEntity because of required properties on TypeEntity<TEnum> so TEntity cannot satisfy new() constraint.
					TEntity seedDataEntry = (TEntity)Activator.CreateInstance(typeof(TEntity))!;

					seedDataEntry.Id = Convert.ToInt32(enumInstance);
					seedDataEntry.Name = enumInstance.ToString();

					return seedDataEntry;
				});

				return seedData;
		}
		catch (Exception ex)
		{
			throw new Exception($"An error occurred while seeding data to {nameof(TEntity)} with {nameof(TEnum)}.", ex);
		}
	}
}
