using LibEternal.JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace LibEternal.Collections
{
	/// <summary>
	///     A <see cref="IEqualityComparer{T}" /> whose <see cref="Equals" /> and <see cref="GetHashCode" /> methods can be set at runtime, instead of having to create a whole new class
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[PublicAPI]
	public class FuncEqualityComparer<T> : IEqualityComparer<T>
	{
		private readonly Func<T, T, bool> equalsFunc;
		private readonly Func<T, int> getHashCodeFunc;

		/// <summary>
		///     Constructs a new <see cref="FuncEqualityComparer{T}" />, using the specified equals function, and optionally a hashcode function.
		/// </summary>
		/// <param name="equalsFunc">The function to call to check objects for equality</param>
		/// <param name="getHashCodeFunc">The function to call to get an object's hashcode. If left null, defaults to calling <c>obj.GetHashCode();</c></param>
		/// <example>
		///     <code>
		/// 		//Create a string hashset that ignores case when comparing for string equality
		///   	HashSet&lt;string&gt; hashSet = new HashSet&lt;string&gt;(
		///  		new[] {"test", "TEST", "TeST"},
		///  		new FuncEqualityComparer&lt;string&gt;(
		///  			//Compare two strings, ignoring their case
		///  			(x, y) =&gt; string.Equals(x, y, StringComparison.CurrentCultureIgnoreCase))
		///  		);
		///   </code>
		/// </example>
		public FuncEqualityComparer([NotNull] Func<T, T, bool> equalsFunc, [CanBeNull] Func<T, int> getHashCodeFunc = null)
		{
			this.equalsFunc = equalsFunc ?? throw new ArgumentNullException(nameof(equalsFunc), "An equals function must be supplied");
			this.getHashCodeFunc = getHashCodeFunc ?? DefaultHashCodeFunc;
		}

		/// <inheritdoc />
		public bool Equals(T x, T y)
		{
			return equalsFunc(x, y);
		}

		/// <inheritdoc />
		public int GetHashCode(T obj)
		{
			return getHashCodeFunc(obj);
		}

		private static int DefaultHashCodeFunc(T obj)
		{
			return obj.GetHashCode();
		}
	}
}