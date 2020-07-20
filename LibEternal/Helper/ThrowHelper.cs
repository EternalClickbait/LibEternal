using LibEternal.JetBrains.Annotations;
using System;

namespace LibEternal.Helper
{
	[PublicAPI]
	public static class ThrowHelper
	{
		public static void CheckRange(int val, int min, int max)
		{
			if (val < min || val > max)
				throw new ArgumentOutOfRangeException(nameof(val), val,
					$"Value {val} was out of valid range {min} to {max}");
		}

		public static void CheckRange<T>(T val, T min, T max) where T : IComparable<T>
		{
			//If val goes before min or after max(when in ascending order)
			if (val.CompareTo(min) < 0 || val.CompareTo(max) > 0)
				throw new ArgumentOutOfRangeException(nameof(val), val,
					$"Value {val} was out of valid range {min} to {max}");
		}
	}
}