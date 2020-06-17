using System.Collections.Generic;
using System.Linq;

namespace LibEternal.Extensions
{
	/// <summary>
	///     A class of extensions methods for numerical values
	/// </summary>
	public static class NumberExtensions
	{
		/// <summary>
		///     Formats a byte <see cref="IEnumerable{T}" /> as hex values
		/// </summary>
		/// <param name="b">The input byte to format</param>
		/// <param name="padZeroes">Whether to pad the output with zeroes e.g. "0x01" instead of "0x1"</param>
		/// <returns>A string representation of the input <paramref name="b" />. E.g. "0x01"</returns>
		public static string FormatAsHexString(this byte b, bool padZeroes = true)
		{
			return "0x" + b.ToString(padZeroes ? "X2" : "X");
		}

		/// <summary>
		///     Formats a byte <see cref="IEnumerable{T}" /> as hex values
		/// </summary>
		/// <param name="bytes">The input bytes to format</param>
		/// <param name="padZeroes">Whether to pad the output with zeroes e.g. "0x01" instead of "0x1"</param>
		/// <returns>A string representation of the input <paramref name="bytes" />. E.g. "0x01, 0x02, 0x56"</returns>
		public static string FormatAsHexString(this IEnumerable<byte> bytes, bool padZeroes = true)
		{
			//Convert each item to hex
			IEnumerable<string> hex = bytes.Select(b => FormatAsHexString(b, padZeroes));
			//Stitch them all together
			return string.Join(", ", hex);
		}

		/// <summary>
		///     Formats a byte <see cref="IEnumerable{T}" /> as numerical values
		/// </summary>
		/// <param name="bytes">The input bytes to format</param>
		/// <returns>A string representation of the input <paramref name="bytes" />. E.g. "67, 68, 69"</returns>
		public static string FormatAsString(this IEnumerable<byte> bytes)
		{
			//Convert each item to a string
			IEnumerable<string> str = bytes.Select(b => b.ToString());
			//Stitch them all together
			return string.Join(", ", str);
		}
	}
}