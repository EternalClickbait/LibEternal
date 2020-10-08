using LibEternal.JetBrains.Annotations;
using System;
using UnityEngine;

#pragma warning disable 1591

namespace LibEternal.Unity
{
	[DefaultExecutionOrder(-1000)]
	[PublicAPI]
	public class EventForwarder : SingletonMonoBehaviour<EventForwarder>
	{
		private void FixedUpdate()
		{
			OnFixedUpdate?.Invoke();
		}

		private void Update()
		{
			OnUpdate?.Invoke();
		}

		public static event Action OnUpdate;

		public static event Action OnFixedUpdate;

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