using System.Collections;
using System.Collections.Generic;

namespace LibEternal.Collections
{
	//From https://stackoverflow.com/a/59412619
	/// <summary>Wrapper for a <see cref="HashSet{T}" /> which allows only for lookup.</summary>
	/// <typeparam name="T">Type of items in the set.</typeparam>
	// ReSharper disable once ClassCanBeSealed.Global
	public class ReadonlySet<T> : IReadonlySet<T>
	{
		/// <summary>
		///     The backing field for this <see cref="ReadonlySet{T}" />
		/// </summary>
		private readonly ISet<T> set;

		/// <summary>Creates new wrapper instance for given set.</summary>
		public ReadonlySet(ISet<T> set)
		{
			this.set = set;
		}

		/// <inheritdoc />
		public int Count => set.Count;

		/// <inheritdoc />
		public bool Contains(T i)
		{
			return set.Contains(i);
		}

		/// <inheritdoc />
		public IEnumerator<T> GetEnumerator()
		{
			return set.GetEnumerator();
		}

		/// <inheritdoc />
		IEnumerator IEnumerable.GetEnumerator()
		{
			return set.GetEnumerator();
		}
	}
}