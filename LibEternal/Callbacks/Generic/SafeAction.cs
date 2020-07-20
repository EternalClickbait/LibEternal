using LibEternal.JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace LibEternal.Callbacks.Generic
{
	/// <summary>
	///     A safe alternative to an <see cref="Action" />
	/// </summary>
	[PublicAPI]
	public class SafeAction
	{
		/// <summary>
		///     The <see cref="List{T}" /> of callbacks
		/// </summary>
		private readonly List<Action> callbacks;

		/// <summary>
		///     The constructor to instantiate a new <see cref="SafeAction" />
		/// </summary>
		/// <param name="callbacks">An optional <see cref="List{T}" /> of <see cref="Action" />s to use as a base</param>
		public SafeAction([CanBeNull] List<Action> callbacks = null)
		{
			this.callbacks = callbacks ?? new List<Action>();
		}

		/// <summary>
		///     An event used to add and remove <see cref="Action" />s from the invocation list
		/// </summary>
		public event Action Event
		{
			add => callbacks.Add(value);
			remove => callbacks.Remove(value);
		}

		/// <summary>
		///     Invokes the <see cref="callbacks" />, catching and returning all thrown <see cref="Exception" />s
		/// </summary>
		/// <returns>A <see cref="List{T}" /> of <see cref="Exception" />s that were thrown during invocation</returns>
		[NotNull]
		public List<Exception> InvokeSafe()
		{
			List<Exception> exceptions = new List<Exception>();

			for (int i = 0; i < callbacks.Count; i++)
				try
				{	
					callbacks[i]?.Invoke();
				}
				//Called if there's an exception in one of the callbacks
				catch (Exception e)
				{
					exceptions.Add(e);
				}

			return exceptions;
		}
	}
}