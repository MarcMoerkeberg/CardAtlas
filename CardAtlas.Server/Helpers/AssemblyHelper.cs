using CardAtlas.Server.Resources.Errors;
using System.Reflection;

namespace CardAtlas.Server.Helpers;

public static class AssemblyHelper
{
	/// <summary>
	/// Returns all classes in the <paramref name="namespaceFilter"/> namespace, that implement one or more interfaces from the same namespace.
	/// </summary>
	/// <param name="namespaceFilter">The namespace to find classes and interfaces from.</param>
	/// <exception cref="ArgumentNullException">Thrown when the <paramref name="namespaceFilter"/> is null or empty.</exception>
	/// <exception cref="InvalidOperationException">Thrown when an error occurs while loading types from the assembly.</exception>
	public static IEnumerable<Type> GetClassesThatImplementInterfaces(string namespaceFilter)
	{
		if(string.IsNullOrWhiteSpace(namespaceFilter))
		{
			throw new ArgumentNullException(nameof(namespaceFilter));
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
					type.Namespace.StartsWith(namespaceFilter, StringComparison.Ordinal) &&
					type.GetInterfaces()
						.Any(@interface => 
							!string.IsNullOrEmpty(@interface.Namespace) && 
							@interface.Namespace.StartsWith(namespaceFilter, StringComparison.Ordinal)
						)
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
