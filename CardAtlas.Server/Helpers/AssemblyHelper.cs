using CardAtlas.Server.Resources.Errors;
using System.Reflection;

namespace CardAtlas.Server.Helpers;

public static class AssemblyHelper
{
	/// <summary>
	/// Returns all classes in the <paramref name="fullNameSpace"/> namespace, that implement one or more interfaces.
	/// </summary>
	/// <exception cref="ArgumentNullException">Thrown when the <paramref name="fullNameSpace"/> is null or empty.</exception>
	/// <exception cref="InvalidOperationException">Thrown when an error occurs while loading types from the assembly.</exception>
	public static IEnumerable<Type> GetClassesThatImplementInterfaces(string fullNameSpace)
	{
		if (string.IsNullOrWhiteSpace(fullNameSpace))
		{
			throw new ArgumentNullException(nameof(fullNameSpace));
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
					type.Namespace.StartsWith(fullNameSpace, StringComparison.Ordinal) &&
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
