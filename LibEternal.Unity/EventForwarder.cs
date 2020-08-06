using LibEternal.JetBrains.Annotations;
using System;
using UnityEngine;

#pragma warning disable 1591

namespace LibEternal.Unity
{
	[DefaultExecutionOrder(-1000)]
	[PublicAPI]
	public class EventForwarder : MonoBehaviour
	{
		private EventForwarder()
		{
		}

		private void FixedUpdate()
		{
			OnFixedUpdate?.Invoke();
		}

		private void Update()
		{
			OnUpdate?.Invoke();
		}

	#region Singleton

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void CreateInstance()
		{
			// ReSharper disable once CommentTypo
			// DontDestroyOnLoad(
			// new GameObject(nameof(EventForwarder))
			// .AddComponent<EventForwarder>());
			Singleton.ForceSingleton<EventForwarder>();
		}

	#endregion

		// ReSharper disable InconsistentNaming
		public static event Action OnUpdate;

		public static event Action OnFixedUpdate;
		// ReSharper restore InconsistentNaming
	}
}