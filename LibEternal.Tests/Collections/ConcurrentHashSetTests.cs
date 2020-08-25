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
			var set = new ConcurrentHashSet<int>();
			Assert.True(set.IsEmpty, "Set should be empty");

			set.Add(0);
			Assert.False(set.IsEmpty, "Set should contain items");
		}

		[Test]
		public void Count_Tests()
		{
			var set = new ConcurrentHashSet<int>(1, 10);
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
			// ReSharper disable once CollectionNeverUpdated.Local
			ICollection<int> collection = new ConcurrentHashSet<int>();
			Assert.False(collection.IsReadOnly);
		}


		//This is just there to cover the ICollection<T> overloads
		[Test]
		public void ICollection_AddAndRemove_ModifyCollection()
		{
			const int dummyNumberMax = 1000;
			ICollection<int> collection = new ConcurrentHashSet<int>();
			Assert.IsEmpty(collection); //No-one wants to be measuring a collection's size when it didn't start off empty

			for (int i = 0; i < dummyNumberMax; i++)
			{
				collection.Add(i);
				//If i is 0, we will already have added it, so we will have i+1 = 1 items
				Assert.AreEqual(i + 1, collection.Count);
			}

			for (int i = dummyNumberMax - 1; i >= 0; i--)
			{
				collection.Remove(i);
				Assert.AreEqual(i, collection.Count);
			}
		}

		[Test]
		public void Clear_SetsSizeToZero()
		{
			var set = new ConcurrentHashSet<int>(Enumerable.Range(0, 1000));
			set.Clear();
			Assert.Zero(set.Count);
		}

		[Test]
		public void TryRemove_ReturnsTrueIfItemRemoved()
		{
			var set = new ConcurrentHashSet<int>(new[] {0});
			//Removing a value should return true if it was actually removed
			Assert.True(set.TryRemove(0));
			Assert.False(set.TryRemove(89443212));
		}

		[Test]
		public void Contains_Tests()
		{
			var set = new ConcurrentHashSet<int>();
			const int iterations = 1000;
			for (int i = 0; i < iterations; i++)
			{
				Assert.False(set.Contains(i)); //Not added yet, so false
				set.Add(i);
				Assert.True(set.Contains(i)); //Added, so true
			}
		}

		[Test]
		public void CopyTo_Tests()
		{
			int[] sourceNumbers = Enumerable.Range(0, 1000).ToArray();

			ICollection<int> collection = new ConcurrentHashSet<int>(sourceNumbers);

			var result = new int[sourceNumbers.Length];

			collection.CopyTo(result, 0);
			CollectionAssert.AreEquivalent(sourceNumbers, result);
		}
	}
}