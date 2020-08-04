//Generated using T4 templates

using LibEternal.JetBrains.Annotations;
using LibEternal.Collections;
using System;
using System.Collections.Generic;

namespace LibEternal.Callbacks.Generic
{
	/// <summary>
	///     A safe alternative to an <see cref="Action{T0}" />
	/// </summary>
	[PublicAPI, System.Runtime.CompilerServices.CompilerGenerated]
	public sealed class SafeAction<T0>
	{
		/// <summary>
		///     The <see cref="HashSet{T}" /> of callbacks.
		/// </summary>
		private readonly HashSet<Action<T0>> callbacks;
		
		/// <summary>
		///     An event used to add and remove <see cref="Action{T0}" />s from the invocation list
		/// </summary>
		public event Action<T0> Event
		{
			add => callbacks.Add(value);
			remove => callbacks.Remove(value);
		}
		
		/// <summary>
		///     A readonly wrapper around the set of callbacks, to allow read-only access
		/// </summary>
		public readonly ReadonlySet<Action<T0>> Callbacks;
		
		/// <summary>
		///     The constructor to instantiate a new <see cref="SafeAction{T0}" />
		/// </summary>
		/// <param name="callbacks">An optional <see cref="List{T}" /> of <see cref="Action" />s to use as a base</param>
		public SafeAction([CanBeNull] IEnumerable<Action<T0>> callbacks = null)
		{
		    // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
		    if(callbacks is null) this.callbacks = new HashSet<Action<T0>>();
		    else this.callbacks = new HashSet<Action<T0>>(callbacks);
		    //Initialize the ReadonlySet field using our newly made HashSet
		    Callbacks = new ReadonlySet<Action<T0>>(this.callbacks);
		}
		
		
		/// <summary>
		///     Invokes the <see cref="callbacks" />, catching and returning all thrown <see cref="Exception" />s
		/// </summary>
		/// <returns>A <see cref="List{T}" /> of <see cref="Exception" />s that were thrown during invocation</returns>
		[NotNull]
		public List<Exception> InvokeSafe(T0 param0)
		{
			List<Exception> exceptions = new List<Exception>();
		
			foreach (Action<T0> callback in callbacks)
			{
				try
				{
					callback?.Invoke(param0);
				}
				//Called if there's an exception in one of the callbacks
				catch (Exception e)
				{
					exceptions.Add(e);
				}
			}
		
			return exceptions;
		}
    }
}