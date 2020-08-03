using System.Collections;
using System.Collections.Generic;

namespace LibEternal.Collections
{
	//From https://stackoverflow.com/a/59412619
	/// <summary>Wrapper for a <see cref="HashSet{T}" /> which allows only for lookup.</summary>
	/// <typeparam name="T">Type of items in the set.</typeparam>
	// ReSharper disable once ClassCanBeSealed.Global
	public class ReadonlyHashSet<T> : IReadonlySet<T>
	{
		/// <summary>
		///     The backing field for this <see cref="ReadonlyHashSet{T}" />
		/// </summary>
		private readonly HashSet<T> hashSet;

		/// <summary>Creates new wrapper instance for given hash set.</summary>
		public ReadonlyHashSet(HashSet<T> set)
		{
			hashSet = set;
		}

		/// <inheritdoc />
		public int Count => hashSet.Count;

		/// <inheritdoc />
		public bool Contains(T i)
		{
			return hashSet.Contains(i);
		}

		/// <inheritdoc />
		public IEnumerator<T> GetEnumerator()
		{
			return hashSet.GetEnumerator();
		}

		/// <inheritdoc />
		IEnumerator IEnumerable.GetEnumerator()
		{
			return hashSet.GetEnumerator();
		}
	}
}