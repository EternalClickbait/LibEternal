using LibEternal.JetBrains.Annotations;
using System;

namespace LibEternal.Helper
{
	/// <summary>
	///     A class used for caching a value and easily updating it.
	/// </summary>
	/// <typeparam name="T">The generic type parameter. Represents what type of object to cache.</typeparam>
	[PublicAPI]
	public class Cached<T>
	{
		private readonly bool allowNull;

		private readonly Func<T, T> updateDelegate;
		private T value = default;

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
			//Pass in the old value, so it can be handled in case it needs to be disposed.
			value = updateDelegate(value);
		}

		/// <summary>
		///     An implicit operator that converts a <see cref="Cached{T}" /> into an object of type <typeparamref name="T" />, by returning the
		///     <paramref name="cached" />'s <see cref="Value" /> property
		/// </summary>
		/// <param name="cached">The <see cref="Cached{T}"/> object to convert into a <typeparamref name="T"/> object</param>
		/// <returns>The currently stored <see cref="Value"/></returns>
		public static implicit operator T(Cached<T> cached)
		{
			return cached.Value;
		}

	#region Constructors

		/// <summary>
		///     A constructor to instantiate a new <see cref="Cached{T}" /> object.
		/// </summary>
		/// <param name="updateDelegate">A <see cref="Delegate" /> to use to obtain the updated value. Should return the new value to be cached.</param>
		public Cached(Func<T> updateDelegate)
		{
			this.updateDelegate = _ => updateDelegate();
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
			this.updateDelegate = _ => updateDelegate();
			value = initialValue;
			this.allowNull = allowNull;
		}

		/// <summary>
		///     <inheritdoc cref="Cached{T}(Func{T})" />
		/// </summary>
		/// <param name="updateDelegate">A <see cref="Delegate" /> to use to obtain the updated value. Takes in the old value as a parameter, where it should be disposed of, and returns the new value to be cached.</param>
		public Cached(Func<T, T> updateDelegate)
		{
			this.updateDelegate = updateDelegate;
			UpdateValue();
		}

		/// <summary>
		///     <inheritdoc cref="Cached{T}(Func{T,T})" />
		/// </summary>
		/// <param name="updateDelegate">
		///     <inheritdoc cref="Cached{T}(Func{T,T})" />
		/// </param>
		/// <param name="initialValue">A value to use as the initial value of the cached object</param>
		/// <param name="allowNull">If null values are allowed. Will have no effect if <typeparamref name="T" /> is a value type</param>
		public Cached(Func<T, T> updateDelegate, T initialValue, bool allowNull = false)
		{
			this.updateDelegate = updateDelegate;
			value = initialValue;
			this.allowNull = allowNull;
		}

	#endregion
	}
}