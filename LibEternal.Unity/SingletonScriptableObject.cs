using LibEternal.JetBrains.Annotations;
using Serilog;
using System;
using UnityEngine;

namespace LibEternal.Unity
{
	/// <summary>
	/// A base class to inherit from to create a singleton <see cref="ScriptableObject"/>. Useful when a manager-type class is required with access to assets (e.g. a prefab manager or audio manager)
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[PublicAPI]
	public abstract class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
	{
		private static T instance = null;

		/// <summary>
		///     Gets the current singleton instance of <typeparamref name="T" />. If it is not found, an <see cref="Exception" /> will be thrown
		/// </summary>
		/// <exception cref="Exception">Thrown when the instance cannot be found</exception>
		[NotNull]
		public static T Instance
		{
			get
			{
				//Instance already assigned, return it
				if (instance) return instance;

				//The instance is not assigned, need to find it
				Log.Debug("Singleton Instance of type {SingletonType} not set, searching...", typeof(T));
				var instances = Resources.FindObjectsOfTypeAll<T>();
				if (instances.Length == 0) //No instances found, search again
					//FindObjectsOfTypeAll<T> only finds the objects loaded in RAM, so we need to try to load all objects into RAM before searching again
					//A slight caveat is that this only works if the prefab is in a resources folder
					instances = Resources.LoadAll<T>("/");

				//All instances should have been found, validate them again
				if (instances.Length == 0)
					throw new Exception($"Singleton of type {typeof(T).Name} not found. Ensure that it is placed in a resources folder");
				if (instances.Length > 1)
					Log.Error("More than one singleton of type {SingletonType} was found ({InstancesCount}): {AllInstances}", typeof(T), instances.Length, instances);
				//Assign the first instance
				instance = instances[0];

				return instance;
			}
		}
	}
}