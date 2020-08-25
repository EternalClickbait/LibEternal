using System.Threading;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

#endif

// ReSharper disable once CheckNamespace
namespace LibEternal.Threading
{
	internal static class UnityThread
	{
		private static SynchronizationContext context;

		// ReSharper disable once ConvertToAutoPropertyWithPrivateSetter
		public static SynchronizationContext Context => context;


#if UNITY_EDITOR
        [InitializeOnLoadMethod]
#endif
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Capture()
		{
			context = SynchronizationContext.Current;
		}
	}
}