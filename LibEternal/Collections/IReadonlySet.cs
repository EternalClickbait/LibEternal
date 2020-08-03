using LibEternal.JetBrains.Annotations;
using System.Collections.Generic;

namespace LibEternal.Collections
{
	//From https://stackoverflow.com/a/59412619
	/// <summary>Represents a readonly set which doesn't allow for the addition of items.</summary>
	/// <typeparam name="T">The type of item in the set.</typeparam>
	[PublicAPI]
	public interface IReadonlySet<T> : IReadOnlyCollection<T>
	{
		/// <summary>Returns true if the set contains given item.</summary>
		public bool Contains(T i);
	}
}