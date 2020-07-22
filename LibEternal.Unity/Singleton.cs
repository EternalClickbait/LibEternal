using LibEternal.JetBrains.Annotations;
using UnityEngine;

namespace LibEternal.Unity
{
	[PublicAPI]
	public static class Singleton
	{
		public static bool IsSingleton<T>(T current) where T : Object
		{
			T[] instances = Object.FindObjectsOfType<T>();
			return instances.Length <= 1 && instances[0] == current;
		}

		public static void ForceSingleton<T>() where T : Component
		{
			T[] instances = Object.FindObjectsOfType<T>();
			if(instances.Length != 0)
				Debug.LogWarning($"Instances of {typeof(T).Name} detected. Destroying...");
			for (int i = 0; i < instances.Length; i++)
			{
				Object.Destroy(instances[i]);
			}

			Object.DontDestroyOnLoad(
				new GameObject(typeof(T).Name)
					.AddComponent<T>());
		}
		
		
	}
}