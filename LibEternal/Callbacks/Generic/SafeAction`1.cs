using LibEternal.JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace LibEternal.Callbacks.Generic
{
	/// <inheritdoc cref="SafeAction" />
	[PublicAPI]
	public class SafeAction<T1>
	{
		/// <inheritdoc cref="SafeAction.callbacks" />
		private readonly HashSet<Action<T1>> callbacks;

		//Temporary
		public IReadOnlyCollection<Action<T1>> Callbacks => callbacks;

		/// <inheritdoc cref="SafeAction(List{Action})" />
		public SafeAction([CanBeNull] IList<Action<T1>> callbacks = null)
		{
			this.callbacks = callbacks != null ? new HashSet<Action<T1>>(callbacks) : new HashSet<Action<T1>>();
		}

		/// <inheritdoc cref="SafeAction.Event" />
		public event Action<T1> Event
		{
			add => callbacks.Add(value);
			remove => callbacks.Remove(value);
		}

		/// <inheritdoc cref="SafeAction.InvokeSafe" />
		[NotNull]
		public List<Exception> InvokeSafe(T1 param1)
		{
			List<Exception> exceptions = new List<Exception>();
			foreach (Action<T1> action in callbacks)
			{
				try
				{
					action?.Invoke(param1);
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