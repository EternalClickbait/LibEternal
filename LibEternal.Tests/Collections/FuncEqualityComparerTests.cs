using LibEternal.Collections;
using NUnit.Framework;
using System;

namespace LibEternal.Tests.Collections
{
	[TestFixture]
	public class FuncEqualityComparerTests
	{
		[Test]
		public void Ctor_ThrowsWhenEqualsFuncNull()
		{
			Assert.Throws<ArgumentNullException>(() => _ = new FuncEqualityComparer<int>(null!));
		}
	}
}