using CardAtlas.Server.Models.Data.Base;
using CardAtlas.Server.Models.Internal;
using Microsoft.AspNetCore.Identity;

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
	/// <exception cref="ArgumentException"/>
	public static List<TEntity> GetEnumSeedData<TEntity, TEnum>()
		where TEntity : TypeEntity<TEnum>
		where TEnum : struct, Enum
	{
		TEnum[] enumValues = Enum.GetValues<TEnum>();
		if (enumValues is not { Length: > 0 })
		{
			throw new ArgumentException($"No values found for enum type {typeof(TEnum).Name}. Ensure enum is defined and has values.");
		}

		List<TEntity> seedData = new(enumValues.Length);

		foreach (TEnum enumValue in enumValues)
		{
			TEntity seedDataEntry = Activator.CreateInstance<TEntity>();
			if (seedDataEntry is null) continue;

			seedDataEntry.Id = Convert.ToInt32(enumValue);
			seedDataEntry.Name = Enum.GetName(typeof(TEnum), enumValue)!;

			seedData.Add(seedDataEntry);
		}

		return seedData;
	}

	/// <summary>
	/// Generatates an <see cref="IdentityRole"/> for every role in <see cref="Roles"/>.
	/// </summary>
	/// <returns>A list of new <see cref="IdentityRole"/>.</returns>
	public static List<IdentityRole> GetIdentitySeedData()
	{
		List<IdentityRole> roles = new(Roles.AllRoles.Length);

		foreach (string roleName in Roles.AllRoles)
		{
			if (string.IsNullOrWhiteSpace(roleName)) continue;

			roles.Add(new IdentityRole
			{
				Name = roleName,
				NormalizedName = roleName.ToUpper()
			});
		}

		return roles;
	}
}
