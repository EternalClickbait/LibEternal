//Generated using T4 templates

using LibEternal.JetBrains.Annotations;
using LibEternal.Collections;
using System;
using System.Collections.Generic;

namespace LibEternal.Callbacks.Generic
{
	/// <summary>
	///     A safe alternative to an <see cref="Func{T0, TReturn}" />
	/// </summary>
	[PublicAPI, System.Runtime.CompilerServices.CompilerGenerated]
	public sealed class SafeFunc<T0, TReturn>
	{
		/// <summary>
		///     The <see cref="HashSet{T}" /> of callbacks.
		/// </summary>
		private readonly HashSet<Func<T0, TReturn>> callbacks;
		
		/// <summary>
		///     An event used to add and remove <see cref="Func{T0, TReturn}" />s from the invocation list
		/// </summary>
		public event Func<T0, TReturn> Event
		{
			add => callbacks.Add(value);
			remove => callbacks.Remove(value);
		}
		
		/// <summary>
		///     A readonly wrapper around the set of callbacks, to allow read-only access
		/// </summary>
		public readonly ReadonlySet<Func<T0, TReturn>> Callbacks;
		
		/// <summary>
		///     The constructor to instantiate a new <see cref="SafeFunc{T0, TReturn}" />
		/// </summary>
		/// <param name="callbacks">An optional <see cref="List{T}" /> of <see cref="Action" />s to use as a base</param>
		public SafeFunc([CanBeNull] IEnumerable<Func<T0, TReturn>> callbacks = null)
		{
		    // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
		    if(callbacks is null) this.callbacks = new HashSet<Func<T0, TReturn>>();
		    else this.callbacks = new HashSet<Func<T0, TReturn>>(callbacks);
		    //Initialize the ReadonlySet field using our newly made HashSet
		    Callbacks = new ReadonlySet<Func<T0, TReturn>>(this.callbacks);
		}
		
		
		/// <summary>
		///     Invokes the <see cref="callbacks" />, catching and returning all thrown <see cref="Exception" />s
		/// </summary>
		/// <returns>A <see cref="List{T}" /> of <see cref="Exception" />s that were thrown during invocation</returns>
		public (List<Exception> Exceptions, List<TReturn> Results) InvokeSafe(T0 param0)
		{
			List<Exception> exceptions = new List<Exception>();
		    List<TReturn> results = new List<TReturn>();
		
			foreach (Func<T0, TReturn> callback in callbacks)
			{
				try
				{
		            if(callback is null) continue;
					TReturn result = callback.Invoke(param0);
		            results.Add(result);
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