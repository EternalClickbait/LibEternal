using LibEternal.JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace LibEternal.Callbacks.Generic
{
	/// <summary>
	///     A safe alternative to an <see cref="Func{TResult}" />
	/// </summary>
	[PublicAPI]
	public class SafeFunc<TReturn>
	{
		/// <summary>
		///     The <see cref="List{T}" /> of callbacks
		/// </summary>
		private readonly List<Func<TReturn>> callbacks;

		/// <summary>
		///     The constructor to instantiate a new <see cref="SafeFunc{TReturn}" />
		/// </summary>
		/// <param name="callbacks">An optional <see cref="List{T}" /> of <see cref="Func{TReturn}" />s to use as a base</param>
		public SafeFunc(List<Func<TReturn>> callbacks = null)
		{	
			this.callbacks = callbacks ?? new List<Func<TReturn>>();
		}

		/// <summary>
		///     An event used to add and remove <see cref="Func{T}" />s from the invocation list
		/// </summary>
		public event Func<TReturn> Event
		{
			add => callbacks.Add(value);
			remove => callbacks.Remove(value);
		}

		/// <summary>
		///     Invokes the <see cref="callbacks" />, catching and returning all thrown <see cref="Exception" />s
		/// </summary>
		/// <returns>A tuple containing a <see cref="List{T}" /> of <see cref="Exception" />s that were thrown during invocation and a <see cref="List{T}" /> of return values</returns>
		public (List<Exception> exceptions, List<TReturn> returns) InvokeSafe()
		{
			List<Exception> exceptions = new List<Exception>();
			List<TReturn> returns = new List<TReturn>();

			for (int i = 0; i < callbacks.Count; i++)
			{
				Func<TReturn> action = callbacks[i];
				try
				{
					returns.Add(action.Invoke());
				}
				//Called if there's an exception in one of the callbacks
				catch (Exception e)
				{
					exceptions.Add(e);
				}
			}

			return (exceptions, returns);
		}
	}
}