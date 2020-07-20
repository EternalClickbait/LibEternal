extern alias Unity;
using LibEternal.JetBrains.Annotations;
using System;
using Unity::UnityEngine;

namespace LibEternal.Unity
{
	[DefaultExecutionOrder(1000)]
	public class LateEventForwarder : MonoBehaviour
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

		// ReSharper disable InconsistentNaming
		[UsedImplicitly] public static event Action OnLateUpdate;

		[UsedImplicitly] public static event Action OnLateFixedUpdate;
		// ReSharper restore InconsistentNaming
	}
}