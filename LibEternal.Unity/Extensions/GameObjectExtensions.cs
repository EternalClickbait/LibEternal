using UnityEngine;

namespace LibEternal.Unity.Extensions
{
	public static class GameObjectExtensions
	{
		//http://answers.unity.com/answers/1190929/view.html
		public static bool IsPrefab(this GameObject obj) => obj.scene.name == null || obj.scene.rootCount == 0;
	}
}