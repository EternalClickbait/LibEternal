using LibEternal.JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace LibEternal.Extensions
{
	/// <summary>
	/// Extensions for enums
	/// </summary>
	[PublicAPI]
	public static class EnumExtensions
	{
		/// <summary>
		/// Returns whether the given enum value is a defined value for its type.
		/// Throws if the type parameter is not an enum type.
		/// </summary>
		public static bool IsDefined<T>(this T enumValue) where T : Enum
		{
			if (typeof(T).BaseType != typeof(Enum)) throw new ArgumentException($"{nameof(T)} must be an enum type.");

			return EnumValueCache<T>.DefinedValues.Contains(enumValue);
		}

		/// <summary>
		/// Statically caches each defined value for each enum type for which this class is accessed.
		/// Uses the fact that static things exist separately for each distinct type parameter.
		/// </summary>
		internal static class EnumValueCache<T>
		{
			public static HashSet<T> DefinedValues { get; }

			static EnumValueCache()
			{
				if (typeof(T).BaseType != typeof(Enum)) throw new Exception($"{nameof(T)} must be an enum type.");

				DefinedValues = new HashSet<T>((T[])Enum.GetValues(typeof(T)));
			}
		}
	}
}