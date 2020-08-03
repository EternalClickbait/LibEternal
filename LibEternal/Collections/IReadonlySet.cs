using System.Collections.Generic;

namespace LibEternal.Collections
{
	/// <summary>Represents a readonly set which doesn't allow for the addition of items.</summary>
	/// <typeparam name="T">The type of item in the set.</typeparam>
	public interface IReadonlySet<T> : IReadOnlyCollection<T>
	{
		/// <summary>Returns true if the set contains given item.</summary>
		public bool Contains(T i);
	}
}