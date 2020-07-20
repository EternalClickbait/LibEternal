using LibEternal.JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace LibEternal.Callbacks.Generic
{
	/// <summary>
	///     A safe alternative to an <see cref="Action" />
	/// </summary>
	[PublicAPI]
	public class SafeFunc<T1, TReturn>
	{
		/// <inheritdoc cref="SafeFunc{TReturn}.callbacks" />
		private readonly List<Func<T1, TReturn>> callbacks;

		/// <inheritdoc cref="SafeFunc{TReturn}(List{Func{TReturn}})" />
		public SafeFunc(List<Func<T1, TReturn>> callbacks = null)
		{
			this.callbacks = callbacks ?? new List<Func<T1, TReturn>>();
		}

		/// <inheritdoc cref="SafeFunc{TReturn}.Event" />
		public event Func<T1, TReturn> Event
		{
			add => callbacks.Add(value);
			remove => callbacks.Remove(value);
		}

		/// <inheritdoc cref="SafeFunc{TReturn}.InvokeSafe" />
		public (List<Exception> exceptions, List<TReturn> results) InvokeSafe(T1 param1)
		{
			List<Exception> exceptions = new List<Exception>();
			List<TReturn> results = new List<TReturn>();

			for (int i = 0; i < callbacks.Count; i++)
			{
				Func<T1, TReturn> action = callbacks[i];
				try
				{
					results.Add(action.Invoke(param1));
				}
				//Called if there's an exception in one of the callbacks
				catch (Exception e)
				{
					exceptions.Add(e);
				}
			}

			return (exceptions, results);
		}
	}
}