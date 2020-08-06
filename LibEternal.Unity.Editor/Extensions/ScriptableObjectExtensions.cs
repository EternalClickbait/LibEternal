using LibEternal.JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace LibEternal.Unity.Editor.Extensions
{
	/// <summary>
	///     An extension class for a <see cref="ScriptableObject" />
	/// </summary>
	[PublicAPI]
	public static class ScriptableObjectExtensions
	{
		/// <summary>
		///     Gets an array of all ScriptableObject assets of type <typeparamref name="T" />
		/// </summary>
		/// <returns></returns>
		[NotNull]
		public static T[] GetAllInstances<T>() where T : ScriptableObject
		{
			string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name); //FindAssets uses tags, check documentation for more info
			T[] array = new T[guids.Length];
			for (int i = 0; i < guids.Length; i++) //probably could get optimized 
			{
				string path = AssetDatabase.GUIDToAssetPath(guids[i]);
				array[i] = AssetDatabase.LoadAssetAtPath<T>(path);
			}

			return array;
		}
	}
}