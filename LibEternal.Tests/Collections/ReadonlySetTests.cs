using LibEternal.Collections;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LibEternal.Tests.Collections
{
	[TestFixture]
	public class ReadonlySetTests
	{
		[Test]
		public void Ctor_ThrowsOnlyWhenSetIsNull()
		{
			Assert.DoesNotThrow(() => _ = new ReadonlySet<int>(new HashSet<int>()), "Constructor with empty HashSet");
			Assert.Throws<ArgumentNullException>(() => _ = new ReadonlySet<int>(null!), "Constructor with null set parameter");
		}

		[Test]
		public void NotSupportedFunctions_Throw()
		{
			Type type = typeof(ReadonlySet<int>);
			var set = new ReadonlySet<int>(new HashSet<int>());

			// ReSharper disable PossibleNullReferenceException
			TargetInvocationException clearException = Assert.Catch<TargetInvocationException>(() =>
				type.GetMethod("Clear", BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase)
					.Invoke(set, null));
			TargetInvocationException addException = Assert.Catch<TargetInvocationException>(() =>
				type.GetMethod("Add", BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase)
					.Invoke(set, new object[] {1}));
			TargetInvocationException removeException = Assert.Catch<TargetInvocationException>(() =>
				type.GetMethod("Remove", BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase)
					.Invoke(set, new object[] {1}));
			// ReSharper restore PossibleNullReferenceException

			//Check that they all threw NotSupportedExceptions
			Assert.IsInstanceOf<NotSupportedException>(clearException.InnerException);
			Assert.IsInstanceOf<NotSupportedException>(addException.InnerException);
			Assert.IsInstanceOf<NotSupportedException>(removeException.InnerException);
		}

		[Test]
		public void CountProperty_IsAccurate()
		{
			const int numTries = 1000;
			var originalSet = new HashSet<int>(numTries);
			var readonlySet = new ReadonlySet<int>(originalSet);

			for (int i = 0; i < numTries; i++)
			{
				originalSet.Add(i);
				Assert.AreEqual(originalSet.Count, readonlySet.Count, "Count property does not equal base set's count");
			}
		}

		[Test]
		public void IsReadonlyProperty_ReturnsTrue()
		{
			Assert.True(new ReadonlySet<int>(new HashSet<int>()).IsReadOnly, "new ReadonlySet<int>(new HashSet<int>()).IsReadOnly");
		}

		[Test]
		public void GetEnumerator_Tests()
		{
			var original = new HashSet<int>(Enumerable.Range(0, 1000));
			var set = new ReadonlySet<int>(original);
			using (IEnumerator<int> enumerator = set.GetEnumerator())
			{
				//See if it returns a non-null enumerator
				Assert.NotNull(enumerator);

				//Also check the IEnumerators are the same
				Assert.AreEqual(original.GetEnumerator(), enumerator, "Enumerator does not equal base enumerator");
			}
		}

		[Test]
		public void Contains_Tests()
		{
			//Half will be tests when the set contains the item, half when it doesn't
			const int totalIterations = 1000;
			var original = new HashSet<int>(totalIterations);
			var readonlySet = new ReadonlySet<int>(original);

			for (int i = 0; i < totalIterations; i++)
			{
				//Only add the number every 2nd iteration
				if (i % 2 == 0)
					original.Add(i);
				Assert.AreEqual(original.Contains(i), readonlySet.Contains(i));
			}
		}

		[Test]
		public void CopyTo_Tests()
		{
			IEnumerable<int> sourceNumbers = Enumerable.Range(0, 1000);
			
			var original = new HashSet<int>(sourceNumbers);
			var readonlySet = new ReadonlySet<int>(original);
			
			var originalResult = new int[original.Count];
			var readonlyResult = new int[original.Count];
			
			original.CopyTo(originalResult);
			readonlySet.CopyTo(readonlyResult);
			CollectionAssert.AreEquivalent(originalResult, readonlyResult);
		}
	}
}