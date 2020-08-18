using NUnit.Framework;
using System;

namespace LibEternal.Tests.Collections.Generic
{
	[TestFixture]
	public class Tests
	{
		[Test]
		public void Test1()
		{
			ShouldBeCovered();
		}

		private static void ShouldBeCovered()
		{
			int a = 4;
			byte b = 7;

			float c = a + b * a * b - b % a - a * b;
			Console.Write(c);
		}
	}
}