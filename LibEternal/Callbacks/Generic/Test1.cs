//Generated using T4 templates

using LibEternal.JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace LibEternal.Callbacks.Generic
{
	/// <summary>
	///     A safe alternative to an <see cref="Action{T0}" />
	/// </summary>
	[PublicAPI, System.Runtime.CompilerServices.CompilerGenerated]
	public sealed class _SafeAction<T0>
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
		///     The constructor to instantiate a new <see cref="SafeAction" />
		/// </summary>
		/// <param name="callbacks">An optional <see cref="List{T}" /> of <see cref="Action" />s to use as a base</param>
		public _SafeAction([CanBeNull] IEnumerable<Action<T0>> callbacks = null)
		{
		    // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
		    if(callbacks is null) this.callbacks = new HashSet<Action<T0>>();
		    else this.callbacks = new HashSet<Action<T0>>(callbacks);
		}
		
