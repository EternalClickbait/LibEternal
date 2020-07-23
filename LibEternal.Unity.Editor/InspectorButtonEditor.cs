using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace LibEternal.Unity.Editor
{
	/// <summary>
	/// An editor class that displays buttons for invoking instance methods of a <see cref="MonoBehaviour"/>
	/// </summary>
	[CustomEditor(typeof(MonoBehaviour), true)]
	public sealed class InspectorButtonEditor : UnityEditor.Editor
	{
		//The flags we use to search for methods
		private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
		private readonly List<(MethodInfo method, object instance)> instanceVoids = new List<(MethodInfo method, object instance)>();

		/// <summary>
		///     A list of suitable static void functions in the class being inspected
		/// </summary>
		private readonly List<MethodInfo> suitableStaticVoids = new List<MethodInfo>();

		/// <summary>
		/// The default (and only) constructor
		/// </summary>
		public InspectorButtonEditor()
		{
			Init();
		}

		/// <summary>
		/// Initializes the editor
		/// </summary>
		private void Init()
		{
			AssemblyReloadEvents.afterAssemblyReload -= Init;
			AssemblyReloadEvents.afterAssemblyReload += Init;

			try
			{
				if (target == null || !target) return;
			}
			catch (NullReferenceException)
			{
				return;
			}

			Type type = target.GetType();

			instanceVoids.Clear();
			suitableStaticVoids.Clear();

			MethodInfo[] allMethods = type.GetMethods(Flags);

			//Loop through all the methods
			for (int i = 0; i < allMethods.Length; i++)
			{
				MethodInfo method = allMethods[i];
				//And try to get attribute marking it to display as a button
				Attribute attribute = method.GetCustomAttribute<InspectorButtonAttribute>();

				//The method doesn't have the attribute
				if (attribute == null) continue;

				if (method.IsStatic) suitableStaticVoids.Add(method);
				else if (!method.IsStatic) instanceVoids.Add((method, target));
			}
		}

		/// <inheritdoc />
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			//Leave a gap if we have any methods to show
			if (instanceVoids.Count + suitableStaticVoids.Count != 0) EditorGUILayout.LabelField("");

			if (instanceVoids.Count != 0)
			{
				EditorGUILayout.LabelField("Instance Methods", EditorStyles.boldLabel);

				for (int i = 0; i < instanceVoids.Count; i++)
				{
					(MethodInfo method, object instance) = instanceVoids[i];

					if (GUILayout.Button(method.Name))
						method.Invoke(instance, new object[0]);
				}
			}
			else
				EditorGUILayout.HelpBox("No Instance Methods", MessageType.None);

			if (suitableStaticVoids.Count != 0)
			{
				EditorGUILayout.LabelField("Static Methods", EditorStyles.boldLabel);

				for (int i = 0; i < suitableStaticVoids.Count; i++)
				{
					MethodInfo method = suitableStaticVoids[i];

					if (GUILayout.Button(method.Name))
						method.Invoke(null, new object[0]);
				}
			}
			else 
				EditorGUILayout.HelpBox("No Static Methods", MessageType.None);
			if(GUILayout.Button("Refresh Method List"))
				Init();
		}
	}
}