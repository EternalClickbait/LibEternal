using System.Collections.Generic;
using UnityEngine;

namespace LibEternal.Unity.Extensions
{
	public static class TransformExtensions
	{
		public static List<Transform> GetAllChildren(this Transform transform)
		{
			var children = new List<Transform>(transform.childCount);
			for (int i = 0; i < transform.childCount; i++) children.Add(transform.GetChild(i));

			return children;
		}
	}
}