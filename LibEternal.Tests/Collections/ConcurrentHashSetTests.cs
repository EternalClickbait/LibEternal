using LibEternal.Collections;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibEternal.Tests.Collections
{
	[TestFixture]
	public class ConcurrentHashSetTests
	{
		[Test]
		public void Ctor_WithValidArgs_DoesNotThrow()
		{
			Assert.DoesNotThrow(() => _ = new ConcurrentHashSet<int>());
			Assert.DoesNotThrow(() => _ = new ConcurrentHashSet<int>(10, 1));
			Assert.DoesNotThrow(() => _ = new ConcurrentHashSet<int>(10, 1, EqualityComparer<int>.Default));
			Assert.DoesNotThrow(() => _ = new ConcurrentHashSet<int>(new int[0]));
			Assert.DoesNotThrow(() => _ = new ConcurrentHashSet<int>(EqualityComparer<int>.Default));
			Assert.DoesNotThrow(() => _ = new ConcurrentHashSet<int>(Enumerable.Range(0, 1000)));
			Assert.DoesNotThrow(() => _ = new ConcurrentHashSet<int>(Enumerable.Repeat(69420, 1000)));
			Assert.DoesNotThrow(() => _ = new ConcurrentHashSet<int>(1000, Enumerable.Range(0, 1000), EqualityComparer<int>.Default));
			Assert.DoesNotThrow(() => _ = new ConcurrentHashSet<int>(1000, Enumerable.Repeat(69420, 1000), EqualityComparer<int>.Default));
		}

		[Test]
		public void Ctor_WithInvalidArgs_Throws()
		{
			Assert.Throws<ArgumentNullException>(() => _ = new ConcurrentHashSet<int>((IEnumerable<int>) null));
			Assert.Throws<ArgumentOutOfRangeException>(() => _ = new ConcurrentHashSet<int>(-1 /*Invalid*/, 0));
			Assert.Throws<ArgumentOutOfRangeException>(() => _ = new ConcurrentHashSet<int>(1, -1 /*Invalid*/));
			Assert.Throws<ArgumentNullException>(() => _ = new ConcurrentHashSet<int>((IEnumerable<int>) null));
			Assert.Throws<ArgumentNullException>(() => _ = new ConcurrentHashSet<int>(1, null!, null));
		}

		[Test]
		public void IsEmpty_Tests()
		{
			ConcurrentHashSet<int> set = new ConcurrentHashSet<int>();
			Assert.True(set.IsEmpty, "Set should be empty");

			set.Add(0);
			Assert.False(set.IsEmpty, "Set should contain items");
		}

		[Test]
		public void Count_Tests()
		{
			ConcurrentHashSet<int> set = new ConcurrentHashSet<int>(1, 10);
			Assert.Zero(set.Count);

			const int itemsToAddPerIteration = 100;
			const int iterations = 10;

			for (int i = 0; i < iterations; i++)
			{
				for (int j = 0; j < itemsToAddPerIteration; j++)
				{
					//Should also be the number of items in the list at the moment
					int num = i * itemsToAddPerIteration + j;
					Assert.AreEqual(num, set.Count);
					set.Add(num);
				}
			}

			Assert.AreEqual(iterations * itemsToAddPerIteration, set.Count);
		}

		[Test]
		public void ICollection_IsReadonly_IsFalse()
		{
			ICollection<int> collection = new ConcurrentHashSet<int>();
			Assert.False(collection.IsReadOnly);
		}

		//This is just there to cover the ICollection<T> overloads
		[Test]
		public void ICollection_Overloads_AreCovered()
		{
			const int dummyNumber = 0;
			ICollection<int> collection = new ConcurrentHashSet<int>();

			collection.Add(dummyNumber);
			Assert.AreEqual(1, collection.Count);

			collection.Remove(dummyNumber);
			Assert.AreEqual(0, collection.Count);

			//Enumerating should never throw
			Assert.DoesNotThrow(() =>
			{
				collection.Add(dummyNumber);
				//Loop over to test the enumerator's MoveNext()
				foreach (int _ in collection)
				{
				}

				collection.Remove(dummyNumber);
			});
			
			collection.Add(dummyNumber);
			//We want to ensure that the collection isn't empty, or clearing it is pointless
			Assert.NotZero(collection.Count);
			collection.Clear();
			
		}
	}
}