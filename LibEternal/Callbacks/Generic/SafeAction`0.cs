//Generated using T4 templates

using LibEternal.JetBrains.Annotations;
using LibEternal.Collections;
using System;
using System.Collections.Generic;

namespace LibEternal.Callbacks.Generic
{
	/// <summary>
	///     A safe alternative to an <see cref="Action" />
	/// </summary>
	[PublicAPI, System.Runtime.CompilerServices.CompilerGenerated]
	public sealed class SafeAction
	{
		/// <summary>
		///     The <see cref="HashSet{T}" /> of callbacks.
		/// </summary>
		private readonly HashSet<Action> callbacks;
		
		/// <summary>
		///     An event used to add and remove <see cref="Action" />s from the invocation list
		/// </summary>
		public event Action Event
		{
			add => callbacks.Add(value);
			remove => callbacks.Remove(value);
		}
		
		public readonly ReadonlyHashSet<Action> Callbacks;
		
		/// <summary>
		///     The constructor to instantiate a new <see cref="SafeAction" />
		/// </summary>
		/// <param name="callbacks">An optional <see cref="List{T}" /> of <see cref="Action" />s to use as a base</param>
		public SafeAction([CanBeNull] IEnumerable<Action> callbacks = null)
		{
		    // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
		    if(callbacks is null) this.callbacks = new HashSet<Action>();
		    else this.callbacks = new HashSet<Action>(callbacks);
		    //Initialize the ReadonlyHashSet field using our newly made HashSet
		    Callbacks = new ReadonlyHashSet<Action>(this.callbacks);
		}
		
		
		/// <summary>
		///     Invokes the <see cref="callbacks" />, catching and returning all thrown <see cref="Exception" />s
		/// </summary>
		/// <returns>A <see cref="List{T}" /> of <see cref="Exception" />s that were thrown during invocation</returns>
		[NotNull]
		public List<Exception> InvokeSafe()
		{
			List<Exception> exceptions = new List<Exception>();
		
			foreach (Action callback in callbacks)
			{
				try
				{
					callback?.Invoke();
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