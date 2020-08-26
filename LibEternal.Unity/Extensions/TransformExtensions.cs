using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

namespace LibEternal.Unity.Extensions
{
	/// <summary>
	///     An extension class for <see cref="Transform" />s and <see cref="RectTransform" />s
	/// </summary>
	[PublicAPI]
	public static class TransformExtensions
	{
		/// <summary>
		///     Gets all the direct children of a <see cref="Transform" />
		/// </summary>
		/// <param name="transform">The <see cref="Transform" /> whose children to get</param>
		/// <returns>A <see cref="List{Transform}" /> of <see cref="Transform" />s that contains all the children of the <paramref name="transform" /></returns>
		[Pure]
		public static List<Transform> GetAllChildren(this Transform transform)
		{
			var children = new List<Transform>(transform.childCount);
			for (int i = 0; i < transform.childCount; i++) children.Add(transform.GetChild(i));

			return children;
		}

		/// <summary>
		///     Removes all children of a <see cref="Transform" />
		/// </summary>
		public static void ClearChildren(this Transform transform)
		{
			List<Transform> children = transform.GetAllChildren();
			for (int i = 0; i < children.Count; i++)
				if (Application.isEditor) Object.DestroyImmediate(children[i].gameObject);
				else Object.Destroy(children[i].gameObject);
		}
	}
}