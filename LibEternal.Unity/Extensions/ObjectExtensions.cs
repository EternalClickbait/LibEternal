using LibEternal.JetBrains.Annotations;
using UnityEngine;

namespace LibEternal.Unity.Extensions
{
	/// <summary>
	/// An extension class for Unity <see cref="Object"/>s
	/// </summary>
	[PublicAPI]
	public static class ObjectExtensions
	{
		/// <summary>
		///     Destroys an object, no matter if in edit mode or play mode
		/// </summary>
		/// <param name="obj">The <see cref="Object" /> to destroy</param>
		public static void SmartDestroy(Object obj)
		{
			//Destroy does nothing in edit mode (need to use DestroyImmediate() instead)
			if (Application.isPlaying)
				Object.Destroy(obj);
			else
				Object.DestroyImmediate(obj);
		}
	}
}