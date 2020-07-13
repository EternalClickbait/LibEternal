extern alias Unity;
using Unity::UnityEngine;

namespace LibEternal.Unity
{
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
			for (int i = 0; i < instances.Length; i++)
			{
				Debug.LogWarning($"Instances of {typeof(T).Name} detected. Destroying...");
				Object.Destroy(instances[i]);
			}

			Object.DontDestroyOnLoad(
				new GameObject(typeof(T).Name)
					.AddComponent<T>());
		}
	}
}