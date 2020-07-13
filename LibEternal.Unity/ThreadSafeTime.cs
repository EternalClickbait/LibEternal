extern alias Unity;
using System.Collections;
using Unity::UnityEngine;

namespace LibEternal.Unity
{
	public class ThreadSafeTime : MonoBehaviour
	{
		//Update every 1 ms. Use a cached wait to avoid allocating every time. (Only 20b/update but better safe than sorry) 
		private static readonly WaitForSeconds Wait = new WaitForSeconds(0.001f);
		public static float Time { get; private set; }

	#region Singleton

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void CreateInstance()
		{
			// DontDestroyOnLoad(
			// new GameObject(nameof(ThreadSafeTime))
			// .AddComponent<ThreadSafeTime>());
			Singleton.ForceSingleton<ThreadSafeTime>();
		}

	#endregion

		private void Awake()
		{
			StartCoroutine(UpdateTimeRoutine());
		}

		private static IEnumerator UpdateTimeRoutine()
		{
			while (true)
			{
				UpdateTime();
				yield return Wait;
			}

			// ReSharper disable once IteratorNeverReturns
		}

	#region Messages

		//Update the time here to ensure that time is always as accurate as possible
		private void Update()
		{
			UpdateTime();
		}

		private void FixedUpdate()
		{
			UpdateTime();
		}

		private void LateUpdate()
		{
			UpdateTime();
		}

		private void OnGUI()
		{
			UpdateTime();
		}

	#endregion

		private static void UpdateTime()
		{
			Time = Unity::UnityEngine.Time.time;
		}
	}
}