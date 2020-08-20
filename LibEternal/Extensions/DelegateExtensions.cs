using LibEternal.JetBrains.Annotations;
using System;

namespace LibEternal.Extensions
{
	/// <summary>
	///     An extension class for delegates
	/// </summary>
	[PublicAPI]
	public static class DelegateExtensions
	{
		/// <summary>
		///     Iterates over the invocation list of a <see cref="Action" /> <see cref="Delegate" />, catching any exceptions during invocation. If an exception is thrown and
		///     <paramref name="onError" /> is null, all errors are suppressed. If it is not null, the method is invoked with the exception passed in. If
		///     <paramref name="onError" /> returns <see langword="true" />, the loop will end and no more actions will be invoked from the list.
		/// </summary>
		/// <param name="thisAction">The <see cref="Action" /> whose invocation list is to be iterated over</param>
		/// <param name="onError">
		///     An optional function returning a <see cref="bool" /> to be called whenever an exception is thrown. If it returns
		///     <see langword="true" />, the loop ends and no more actions are invoked.
		/// </param>
		public static void InvokeCatchingErrors([NotNull] this Action thisAction, [CanBeNull] Func<Exception, bool> onError = null)
		{
			foreach (Delegate del in thisAction.GetInvocationList())
				try
				{
					//Try invoking the next action in the list
					Action action = del as Action;
					action?.Invoke();
				}
				catch (Exception e)
				{
					//If the OnError function is null, assume the user wants to catch all errors
					if (onError is null) continue;
					//If it returns true, stop invoking the rest of the functions
					if (onError(e)) return;
				}
		}

		/// <summary>
		///     Iterates over the invocation list of a <see cref="Action" /> <see cref="Delegate" />, catching any exceptions during invocation. If an exception is thrown and
		///     <paramref name="onError" /> is null, all errors are suppressed. If it is not null, the method is invoked with the exception passed in. If
		///     <paramref name="onError" /> returns <see langword="true" />, the loop will end and no more actions will be invoked from the list.
		/// </summary>
		/// <param name="thisAction">The <see cref="Action" /> whose invocation list is to be iterated over</param>
		/// <param name="param">The parameter to pass into the delegate</param>
		/// <param name="onError">
		///     An optional function returning a <see cref="bool" /> to be called whenever an exception is thrown. If it returns
		///     <see langword="true" />, the loop ends and no more actions are invoked.
		/// </param>
		public static void InvokeCatchingErrors<TParam>([NotNull] this Action<TParam> thisAction, TParam param,
		                                                [CanBeNull] Func<Exception, bool> onError = null)
		{
			foreach (Delegate del in thisAction.GetInvocationList())
				try
				{
					//Try invoking the next action in the list
					var action = del as Action<TParam>;
					action?.Invoke(param);
				}
				catch (Exception e)
				{
					//If the OnError function is null, assume the user wants to catch all errors
					if (onError is null) continue;
					//If it returns true, stop invoking the rest of the functions
					if (onError(e)) return;
				}
		}
	}
}