
//Generated using T4 templates

using LibEternal.JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace LibEternal.Callbacks.Generic
{
	/// <summary>
	///     A safe alternative to an <see cref="Action" />
	/// </summary>
	[PublicAPI, System.Runtime.CompilerServices.CompilerGenerated]
	public sealed class _SafeAction
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
		
		/// <summary>
		///     The constructor to instantiate a new <see cref="SafeAction" />
		/// </summary>
		/// <param name="callbacks">An optional <see cref="List{T}" /> of <see cref="Action" />s to use as a base</param>
		public _SafeAction([CanBeNull] IEnumerable<Action> callbacks = null)
		{
		    // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
		    if(callbacks is null) this.callbacks = new HashSet<Action>();
		    else this.callbacks = new HashSet<Action>(callbacks);
		}
		
