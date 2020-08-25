using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

namespace LibEternal.Unity.Extensions
{
	[PublicAPI]
	public static class TransformExtensions
	{
		[Pure]
		public static List<Transform> GetAllChildren(this Transform transform)
		{
			var children = new List<Transform>(transform.childCount);
			for (int i = 0; i < transform.childCount; i++) children.Add(transform.GetChild(i));

			return children;
		}

		/// <summary>
		/// Removes all children of a <see cref="Transform"/>
		/// </summary>
		public static void ClearChildren(this Transform transform)
		{
			List<Transform> children = transform.GetAllChildren();
			for (int i = 0; i < children.Count; i++)
			{
				if (Application.isEditor) Object.DestroyImmediate(children[i].gameObject);
				else Object.Destroy(children[i].gameObject);
			}
		}
	}
}