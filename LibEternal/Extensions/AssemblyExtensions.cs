using LibEternal.JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LibEternal.Extensions
{
	/// <summary>
	///     An extension class for the <see cref="Assembly" /> class.
	/// </summary>
	[PublicAPI]
	public static class AssemblyExtensions
	{
		/// <summary>
		///     Gets all the types in an assembly, skipping any types where .Net can't load the assemblies required for the type
		/// </summary>
		/// <param name="assembly"></param>
		/// <returns></returns>
		public static IEnumerable<Type> GetTypesSafe(this Assembly assembly)
		{
			try
			{
				//This sometimes throws
				return assembly.GetTypes();
			}
			//This can occur if .Net can't load one of the assemblies required for a type.
			catch (ReflectionTypeLoadException e)
			{
				//Return the types we managed to load. Failed ones are null, so remove them
				return e.Types.Where(t => t != null);
			}
		}

		public static IEnumerable<Type> GetInheritedTypes(this Assembly assembly, Type baseType, bool includeAbstract = false, bool includeInterfaces = false)
		{
			return assembly.GetTypesSafe()
				//
				.Where(type => type.IsSubclassOf(baseType))
				/*
				 * Here's a little truth table I created for what I want this function to select
				 * type.IsAbstract 	|	includeAbstract		|	Include(Out)
				 * N				|	N					|	Y
				 * N				|	Y					|	Y
				 * Y				|	N					|	N
				 * Y				|	Y					|	Y
				 *
				 * We only skip the type when it is abstract but we don't want to include them (type.IsAbstract && !includeAbstract).
				 * So we can just negate it to get the types we want to keep
				 * `!(type.IsAbstract && !includeAbstract)` or `!type.IsAbstract || includeAbstract`
				 */
				.Where(type => !type.IsAbstract || includeAbstract)
				//Also do the same for interfaces
				.Where(type => !type.IsInterface || includeInterfaces);



		}
	}
}