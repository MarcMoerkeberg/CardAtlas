using CardAtlas.Server.Resources.Errors;
using System.Reflection;

namespace CardAtlas.Server.Helpers;

public static class AssemblyHelper
{
	/// <summary>
	/// Returns all classes in the <paramref name="namespaceSuffix"/> namespace, that implements one or more interfaces.
	/// </summary>
	/// <exception cref="ArgumentNullException">Thrown when the <paramref name="namespaceSuffix"/> is null or empty.</exception>
	/// <exception cref="InvalidOperationException">Thrown when an error occurs while loading types from the assembly.</exception>
	public static IEnumerable<Type> GetClassesThatImplementInterfaces(string namespaceSuffix, string namespacePrefix = "CardAtlas.Server")
	{
		if (string.IsNullOrWhiteSpace(namespaceSuffix))
		{
			throw new ArgumentNullException(nameof(namespaceSuffix));
		}

		try
		{
			IEnumerable<Type> servicesWithTypes = Assembly
				.GetExecutingAssembly()
				.GetTypes()
				.Where(type =>
					type.IsClass &&
					!type.IsAbstract &&
					!string.IsNullOrEmpty(type.Namespace) &&
					type.Namespace.StartsWith(namespacePrefix, StringComparison.Ordinal) &&
					type.Namespace.EndsWith(namespaceSuffix, StringComparison.Ordinal) &&
					type.GetInterfaces().Any()
				);

			return servicesWithTypes;
		}
		catch (ReflectionTypeLoadException ex)
		{
			throw new InvalidOperationException(Errors.FailedLoadingTypesFromAssembly, ex);
		}
		catch (TargetInvocationException ex)
		{
			throw new InvalidOperationException(Errors.FailedGettingInterfaces, ex);
		}
	}
}
