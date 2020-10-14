using JetBrains.Annotations;
using System;
using System.Reflection;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace LibEternal.Unity.Extensions
{
	/// <summary>
	/// A set of extensions for <see cref="UnityEvent"/>s
	/// </summary>
	[PublicAPI]
	public static class UnityEventExtensions
	{
		/// <summary>
		///     Gets all the listeners of a <see cref="UnityEvent" />.
		/// </summary>
		/// <param name="event">The <see cref="UnityEvent" /> whose methods to get</param>
		/// <returns>
		///     Returns an <see cref="System.Array" /> of all persistent methods on the <paramref name="event" />
		/// </returns>
		///
		///  See <see cref="GetPersistentMethodAndTarget"/> for more information.
		[NotNull]
		[Pure]
		public static (MethodInfo MethodInfo, Object Target)[] GetPersistentMethodsAndTargets(this UnityEvent @event)
		{
			int count = @event.GetPersistentEventCount();
			var listeners = new (MethodInfo MethodInfo, Object Target)[count];
			for (int i = 0; i < count; i++) listeners[i] = @event.GetPersistentMethodAndTarget(i);
			return listeners;
		}

		/// <summary>
		///     <para>
		///         Gets the <see cref="MethodInfo" /> and <see cref="UnityEngine.Object" /> that corresponds to a persistent listener at the specified
		///         <paramref name="index" />.
		///     </para>
		/// </summary>
		/// <remarks>
		///     <para>
		///         Invoke the method by calling <see cref="MethodInfo.Invoke(object, object[])" /> on the returned <see cref="MethodInfo" />, passing in the
		///         <c>Target</c> and <c>null</c> for the parameters.
		///     </para>
		///     <para>
		///         No validation is performed by this method, so any <see cref="System.Exception" />s thrown by the called code will be passed up to the caller.
		///     </para>
		/// </remarks>
		/// <example>
		///     <code>
		///  	//Step 1: Get the MethodInfo and target
		///  	(MethodInfo method, Object target) = new UnityEvent().GetPersistentMethodAndTarget(2);
		///  	//Step 2: Invoke the method
		///  	method.Invoke(target, null);
		///  	//Step 3: Profit????
		///   	</code>
		/// </example>
		/// <exception cref="System.ArgumentOutOfRangeException">Thrown (by Unity code) when the specified <paramref name="index" /> is out of range (&lt; 0 or &gt; the listener count)</exception>
		/// <param name="event">The <see cref="UnityEvent" /> whose persistent method and target to get.</param>
		/// <param name="index">The index of the persistent method to get. This index is not validated for performance reasons (see the Remarks section for details)</param>
		/// <returns>A named <see cref="System.Tuple{T1,T2}" /> that contains both the <see cref="MethodInfo" /> and the <see cref="Object" /> target used to invoke it.</returns>
		[Pure]
		public static (MethodInfo Method, Object Target) GetPersistentMethodAndTarget(this UnityEvent @event, int index)
		{
			Object obj = @event.GetPersistentTarget(index);
			MethodInfo method = UnityEventBase.GetValidMethodInfo(obj, @event.GetPersistentMethodName(index), Array.Empty<Type>());
			return (method, obj);
		}
	}
}