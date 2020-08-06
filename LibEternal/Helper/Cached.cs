using LibEternal.JetBrains.Annotations;
using System;

namespace LibEternal.Helper
{
	//URGENT: Need to be able to dispose of old value somehow
#error Add dispose of old value
	/// <summary>
	///     A class used for caching a value and easily updating it.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[PublicAPI]
	public class Cached<T>
	{
		private readonly bool allowNull;

		private readonly Func<T> updateDelegate;
		private T value = default;

		/// <summary>
		///     A constructor to instantiate a new <see cref="Cached{T}" /> object
		/// </summary>
		/// <param name="updateDelegate">A <see cref="Delegate" /> to use to obtain the updated value</param>
		public Cached(Func<T> updateDelegate)
		{
			this.updateDelegate = updateDelegate;
			UpdateValue();
		}

		/// <summary>
		///     <inheritdoc cref="Cached{T}(Func{T})" />
		/// </summary>
		/// <param name="updateDelegate">
		///     <inheritdoc cref="Cached{T}(Func{T})" />
		/// </param>
		/// <param name="initialValue">A value to use as the initial value of the cached object</param>
		/// <param name="allowNull">If null values are allowed. Will have no effect if <typeparamref name="T" /> is a value type</param>
		public Cached(Func<T> updateDelegate, T initialValue, bool allowNull = false)
		{
			this.updateDelegate = updateDelegate;
			value = initialValue;
			this.allowNull = allowNull;
		}

		/// <summary>
		///     Gets the cached value of this instance
		/// </summary>
		public T Value
		{
			get
			{
				//If we aren't allow to have a null value, and the value is null, try updating it
				if (!allowNull && value is null)
					UpdateValue();

				return value;
			}
		}

		/// <summary>
		///     Updates the cached value, and returns it.
		/// </summary>
		/// <returns>Returns an up-to-date value</returns>
		public T UpdateAndGetValue()
		{
			UpdateValue();
			return Value;
		}

		/// <summary>
		///     Updates the stored value
		/// </summary>
		public void UpdateValue()
		{
			value = updateDelegate();
		}
	}
}