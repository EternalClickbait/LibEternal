using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace LibEternal.Callbacks.Generic
{
	/// <inheritdoc cref="SafeAction" />
	[PublicAPI]
	public class SafeAction<T1, T2>
	{
		/// <inheritdoc cref="SafeAction.callbacks" />
		private readonly List<Action<T1, T2>> callbacks;

		/// <inheritdoc cref="SafeAction(List{Action})" />
		public SafeAction(List<Action<T1, T2>> callbacks = null)
		{
			this.callbacks = callbacks ?? new List<Action<T1, T2>>();
		}

		/// <inheritdoc cref="SafeAction.Event" />
		public event Action<T1, T2> Event
		{
			add => callbacks.Add(value);
			remove => callbacks.Remove(value);
		}

		/// <inheritdoc cref="SafeAction.InvokeSafe" />
		public List<Exception> InvokeSafe(T1 param1, T2 param2)
		{
			List<Exception> exceptions = new List<Exception>();
			for (int i = 0; i < callbacks.Count; i++)
			{
				Action<T1, T2> action = callbacks[i];
				try
				{
					action?.Invoke(param1, param2);
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