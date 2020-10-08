using LibEternal.JetBrains.Annotations;
using System;
using UnityEngine;

#pragma warning disable 1591

namespace LibEternal.Unity
{
	[DefaultExecutionOrder(1000)]
	public sealed class LateEventForwarder : SingletonMonoBehaviour<LateEventForwarder>
	{
		private void FixedUpdate()
		{
			OnLateFixedUpdate?.Invoke();
		}

		private void LateUpdate()
		{
			OnLateUpdate?.Invoke();
		}
		
		[UsedImplicitly] public static event Action OnLateUpdate;

		[UsedImplicitly] public static event Action OnLateFixedUpdate;

		/// <inheritdoc />
		protected override void SingletonAwakened()
		{
		}

		/// <inheritdoc />
		protected override void SingletonStarted()
		{
		}

		/// <inheritdoc />
		protected override void SingletonDestroyed()
		{
		}
	}
}