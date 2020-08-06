using LibEternal.JetBrains.Annotations;
using System;
using UnityEngine;

#pragma warning disable 1591

namespace LibEternal.Unity
{
	[DefaultExecutionOrder(1000)]
	public sealed class LateEventForwarder : MonoBehaviour
	{
		private void FixedUpdate()
		{
			OnLateFixedUpdate?.Invoke();
		}

		private void LateUpdate()
		{
			OnLateUpdate?.Invoke();
		}

	#region Singleton

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void CreateInstance()
		{
			Singleton.ForceSingleton<LateEventForwarder>();
		}

	#endregion

		[UsedImplicitly] public static event Action OnLateUpdate;

		[UsedImplicitly] public static event Action OnLateFixedUpdate;
	}
}