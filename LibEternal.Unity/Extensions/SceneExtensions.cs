using LibEternal.JetBrains.Annotations;
using UnityEngine.SceneManagement;

namespace LibEternal.Unity.Extensions
{
	[PublicAPI]
	public static class SceneExtensions
	{
		/// <summary>
		/// A better version of <see cref="Scene.ToString"/> that actually returns info about the scene (ame, path and index)
		/// </summary>
		/// <param name="scene"></param>
		/// <returns></returns>
		public static string ToStringNice(this Scene scene)
		{
			return $"{scene.name} ({scene.path} # {scene.buildIndex})";
		}

		public static bool IsInvalidScene(this Scene scene)
		{
			/*
			 * Valid Scene:
			 * {
			 *   "handle": -96740,
			 *   "path": "Assets/Scenes/UI Prefab Editor.unity",
			 *   "name": "2. Create Game Panel",
			 *   "isLoaded": true,
			 *   "buildIndex": -1,
			 *   "isDirty": false,
			 *   "rootCount": 4,
			 *   "isSubScene": false
			 * }
			 * 
			 * Invalid Scene (`default(Scene)`):
			 * {
			 *   "handle": 0,
			 *   "isLoaded": false,
			 *   "buildIndex": -1,
			 *   "isDirty": false,
			 *   "rootCount": 0,
			 *   "isSubScene": true
			 * }
			 */

			return scene.handle == 0
			       || string.IsNullOrEmpty(scene.path)
			       || string.IsNullOrEmpty(scene.name)
			       || scene == default;
		}
	}
}