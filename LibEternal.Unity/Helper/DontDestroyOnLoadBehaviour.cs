using UnityEngine;

namespace LibEternal.Unity.Helper
{
	/// <summary>
	/// When placed on a <see cref="GameObject"/>, marks that <see cref="GameObject"/> to not be destroyed when a scene is loaded.
	/// </summary>
	public sealed class DontDestroyOnLoadBehaviour : MonoBehaviour
	{
		private void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}
	}
}