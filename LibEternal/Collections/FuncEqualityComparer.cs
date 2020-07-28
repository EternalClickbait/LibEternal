using LibEternal.JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace LibEternal.Collections
{
	public class FuncEqualityComparer<T> : IEqualityComparer<T>
	{
		private readonly Func<T, T, bool> equalsFunc;
		private readonly Func<T, int> getHashCodeFunc;

		public FuncEqualityComparer([NotNull] Func<T, T, bool> equalsFunc, [CanBeNull] Func<T, int> getHashCodeFunc = null)
		{
			this.equalsFunc = equalsFunc ?? throw new ArgumentNullException(nameof(equalsFunc), "An equals function must be supplied");
			this.getHashCodeFunc = getHashCodeFunc;
		}

		public bool Equals(T x, T y)
		{
			return equalsFunc(x, y);
		}

		public int GetHashCode(T obj)
		{
			return getHashCodeFunc(obj);
		}
	}
}