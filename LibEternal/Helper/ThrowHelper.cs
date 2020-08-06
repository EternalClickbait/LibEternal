using LibEternal.JetBrains.Annotations;
using System;

namespace LibEternal.Helper
{
	/// <summary>
	///     A class that contains functions to assist with throwing <see cref="Exception" />s
	/// </summary>
	[PublicAPI]
	public static class ThrowHelper
	{
		/// <summary>
		///     Validates the input <paramref name="value" />, the ensure it is within the range of <paramref name="min" /> to <paramref name="max" />
		/// </summary>
		/// <param name="value">The value to validate</param>
		/// <param name="min">The minimum value allowed for the <paramref name="value" /></param>
		/// <param name="max">The maximum value allowed for the <paramref name="value" /></param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if the <paramref name="value" /> is out of range</exception>
		public static void CheckRange(int value, int min, int max)
		{
			if (value < min || value > max)
				throw new ArgumentOutOfRangeException(nameof(value), value,
					$"Value {value} was out of valid range {min} to {max}");
		}
	}
}