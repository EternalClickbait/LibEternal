﻿using LibEternal.JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LibEternal.Collections
{
	//From https://stackoverflow.com/a/59412619
	/// <summary>Wrapper for a <see cref="HashSet{T}" /> which allows only for lookup.</summary>
	/// <typeparam name="T">Type of items in the set.</typeparam>
	// ReSharper disable once ClassCanBeSealed.Global
	public class ReadonlySet<T> : IReadonlySet<T>, ICollection<T>
	{
		/// <summary>
		///     The backing field for this <see cref="ReadonlySet{T}" />
		/// </summary>
		private readonly ISet<T> set;

		/// <summary>Creates new wrapper instance for given set.</summary>
		public ReadonlySet([NotNull] ISet<T> set)
		{
			this.set = set ?? throw new ArgumentNullException(nameof(set));
		}

		/// <summary>
		/// A non-supported method. Every call will always throw a <see cref="NotSupportedException"/>
		/// </summary>
		/// <exception cref="NotSupportedException">Removing items from a <see cref="ReadonlySet{T}"/> is not supported</exception>
		[Obsolete("Not Supported", true)]
		public bool Remove(T item)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc cref="IReadOnlyCollection{T}.Count" />
		public int Count => set.Count;

		/// <inheritdoc />
		public bool IsReadOnly => true;

		/// <summary>
		/// A non-supported method. Every call will always throw a <see cref="NotSupportedException"/>
		/// </summary>
		/// <exception cref="NotSupportedException">Adding items to a <see cref="ReadonlySet{T}"/> is not supported</exception>
		[Obsolete("Not Supported", true)]
		public void Add(T item)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// A non-supported method. Every call will always throw a <see cref="NotSupportedException"/>
		/// </summary>
		/// <exception cref="NotSupportedException">Clearing a <see cref="ReadonlySet{T}"/> is not supported</exception>
		[Obsolete("Not Supported", true)]
		public void Clear()
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc cref="IReadonlySet{T}.Contains" />
		public bool Contains(T i)
		{
			return set.Contains(i);
		}

		/// <inheritdoc />
		public void CopyTo(T[] array, int startIndex = 0)
		{
			set.CopyTo(array, startIndex);
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