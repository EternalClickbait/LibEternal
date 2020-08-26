using JetBrains.Annotations;
using UnityEngine;

namespace LibEternal.Unity.Extensions
{
	/// <summary>
	///     An extension class for <see cref="GameObject" />s
	/// </summary>
	[PublicAPI]
	public static class GameObjectExtensions
	{
		/// <summary>
		///     Used to check if a given <see cref="GameObject" /> is a prefab
		/// </summary>
		/// <param name="obj">The <see cref="GameObject" /> to check</param>
		/// <returns>True if the object is a prefab, otherwise false</returns>
		//http://answers.unity.com/answers/1190929/view.html
		public static bool IsPrefab(this GameObject obj)
		{
			return obj.scene.name == null || obj.scene.rootCount == 0;
		}
	}
}