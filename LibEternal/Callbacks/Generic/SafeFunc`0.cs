//Generated using T4 templates

using LibEternal.JetBrains.Annotations;
using LibEternal.Collections;
using System;
using System.Collections.Generic;

namespace LibEternal.Callbacks.Generic
{
	/// <summary>
	///     A safe alternative to an <see cref="Func{TReturn}" />
	/// </summary>
	[PublicAPI, System.Runtime.CompilerServices.CompilerGenerated]
	public sealed class SafeFunc<TReturn>
	{
		/// <summary>
		///     The <see cref="HashSet{T}" /> of callbacks.
		/// </summary>
		private readonly HashSet<Func<TReturn>> callbacks;
		
		/// <summary>
		///     An event used to add and remove <see cref="Func{TReturn}" />s from the invocation list
		/// </summary>
		public event Func<TReturn> Event
		{
			add => callbacks.Add(value);
			remove => callbacks.Remove(value);
		}
		
		/// <summary>
		///     A readonly wrapper around the set of callbacks, to allow read-only access
		/// </summary>
		public readonly ReadonlySet<Func<TReturn>> Callbacks;
		
		/// <summary>
		///     The constructor to instantiate a new <see cref="SafeFunc{TReturn}" />
		/// </summary>
		/// <param name="callbacks">An optional <see cref="List{T}" /> of <see cref="Action" />s to use as a base</param>
		public SafeFunc([CanBeNull] IEnumerable<Func<TReturn>> callbacks = null)
		{
		    // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
		    if(callbacks is null) this.callbacks = new HashSet<Func<TReturn>>();
		    else this.callbacks = new HashSet<Func<TReturn>>(callbacks);
		    //Initialize the ReadonlySet field using our newly made HashSet
		    Callbacks = new ReadonlySet<Func<TReturn>>(this.callbacks);
		}
		
		
		/// <summary>
		///     Invokes the <see cref="callbacks" />, catching and returning all thrown <see cref="Exception" />s
		/// </summary>
		/// <returns>A <see cref="List{T}" /> of <see cref="Exception" />s that were thrown during invocation</returns>
		public (List<Exception> Exceptions, List<TReturn> Results) InvokeSafe()
		{
			List<Exception> exceptions = new List<Exception>();
		    List<TReturn> results = new List<TReturn>();
		
			foreach (Func<TReturn> callback in callbacks)
			{
				try
				{
		            if(callback is null) continue;
					TReturn result = callback.Invoke();
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