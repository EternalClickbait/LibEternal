using JetBrains.Annotations;
using System;

namespace LibEternal.Extensions
{
	[PublicAPI]
	public static class RandomExtensions
	{
		public static long RandomLong(this Random rnd)
		{
			byte[] buffer = new byte[8];
			rnd.NextBytes (buffer);
			return BitConverter.ToInt64(buffer, 0);
		}

		public static long RandomLong(this Random rnd, long min, long max)
		{
			EnsureMinLEQMax(ref min, ref max);
			long numbersInRange = unchecked(max - min + 1);
			if (numbersInRange < 0)
				throw new ArgumentException("Size of range between min and max must be less than or equal to Int64.MaxValue");

			long randomOffset = RandomLong(rnd);
			if (IsModuloBiased(randomOffset, numbersInRange))
				return RandomLong(rnd, min, max); // Try again
			else
				return min + PositiveModuloOrZero(randomOffset, numbersInRange);
		}

		private static bool IsModuloBiased(long randomOffset, long numbersInRange)
		{
			long greatestCompleteRange = numbersInRange * (long.MaxValue / numbersInRange);
			return randomOffset > greatestCompleteRange;
		}

		private static long PositiveModuloOrZero(long dividend, long divisor)
		{
			Math.DivRem(dividend, divisor, out long mod);
			if(mod < 0)
				mod += divisor;
			return mod;
		}

		// ReSharper disable once InconsistentNaming
		private static void EnsureMinLEQMax(ref long min, ref long max)
		{
			if(min <= max)
				return;
			long temp = min;
			min = max;
			max = temp;
		}
	}
}