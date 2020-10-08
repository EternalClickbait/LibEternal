using LibEternal.JetBrains.Annotations;
using System.Collections;
using UnityEngine;

#pragma warning disable 1591

namespace LibEternal.Unity
{
	[PublicAPI]
	public sealed class ThreadSafeTime : SingletonMonoBehaviour<ThreadSafeTime>
	{
		
		//Update every 1 ms. Use a cached wait to avoid allocating every time. (Only 20b/update but better safe than sorry) 
		private static readonly WaitForSeconds Wait = new WaitForSeconds(0.001f);
		
		public static float Time { get; private set; }

		/// <inheritdoc />
		protected override void SingletonAwakened()
		{
			StartCoroutine(UpdateTimeRoutine());
		}

		/// <inheritdoc />
		protected override void SingletonStarted()
		{
			UpdateTime();
		}

		/// <inheritdoc />
		protected override void SingletonDestroyed()
		{
			UpdateTime();
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

		private static void UpdateTime()
		{
			Time = UnityEngine.Time.time;
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
	}
}