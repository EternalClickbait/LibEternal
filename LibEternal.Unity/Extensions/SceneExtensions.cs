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
	}
}