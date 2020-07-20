using LibEternal.JetBrains.Annotations;
using System.Collections.Generic;
using System.Text;

namespace LibEternal.Extensions
{
	[PublicAPI]
	public static class ArrayExtensions
	{
		/// <summary>
		/// Returns if the two input arrays have the same contents
		/// </summary>
		/// <param name="array1">The first array</param>
		/// <param name="array2">The second array</param>
		/// <param name="comparer">The <see cref="IEqualityComparer{T}"/></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static bool ContentsEqual<T>(this T[] array1, T[] array2, IEqualityComparer<T> comparer = null)
		{
			if (ReferenceEquals(array1, array2))
				return true;

			if (array1 == null || array2 == null)
				return false;

			if (array1.Length != array2.Length)
				return false;

			if (comparer == null)
				comparer = EqualityComparer<T>.Default;
			
			for (int i = 0; i < array1.Length; i++)
			{
				if (!comparer.Equals(array1[i], array2[i])) return false;
			}

			return true;
		}

		/// <summary>
		/// Encodes a <see cref="string"/>  using <see cref="Encoding.UTF8"/> <see cref="Encoding"/>, turning it into a <see cref="byte"/> <see cref="System.Array"/>
		/// </summary>
		/// <param name="str">The input <see cref="string"/> to encode</param>
		/// <returns></returns>
		public static byte[] ToUtf8(this string str)
		{
			return Encoding.UTF8.GetBytes(str);
		}

		/// <summary>
		/// Decodes the <see cref="byte"/>s in a <see cref="byte"/> array and converts them to a string, using <see cref="Encoding.UTF8"/> <see cref="Encoding"/>.
		/// </summary>
		/// <param name="b">The input <see cref="System.Array"/> to decode</param>
		/// <returns></returns>
		public static string FromUtf8(this byte[] b)
		{
			return Encoding.UTF8.GetString(b);
		}
	}
}