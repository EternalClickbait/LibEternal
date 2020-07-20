using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace LibEternal.Unity.Editor.Editor
{
	[CustomEditor(typeof(MonoBehaviour), true)]
	public class InspectorButtonEditor : UnityEditor.Editor
	{
		//The flags we use to search for methods
		private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
		private readonly List<(MethodInfo method, object instance)> instanceVoids = new List<(MethodInfo method, object instance)>();

		//List of void functions in class
		private readonly List<MethodInfo> staticVoids = new List<MethodInfo>();

		public InspectorButtonEditor()
		{
			Init();
		}

		//Initializes the editor
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
			staticVoids.Clear();

			MethodInfo[] allMethods = type.GetMethods(Flags);

			//Loop through all the methods
			for (int i = 0; i < allMethods.Length; i++)
			{
				MethodInfo method = allMethods[i];
				//And try to get attribute marking it to display as a button
				Attribute attribute = method.GetCustomAttribute<InspectorButtonAttribute>();

				//The method doesn't have the attribute
				if (attribute == null) continue;

				if (method.IsStatic) staticVoids.Add(method);
				else if (!method.IsStatic) instanceVoids.Add((method, target));
			}
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			//Leave a gap if we have any methods to show
			if (instanceVoids.Count + staticVoids.Count != 0) EditorGUILayout.LabelField("");

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

			if (staticVoids.Count != 0)
			{
				EditorGUILayout.LabelField("Static Methods", EditorStyles.boldLabel);

				for (int i = 0; i < staticVoids.Count; i++)
				{
					MethodInfo method = staticVoids[i];

					if (GUILayout.Button(method.Name))
						method.Invoke(null, new object[0]);
				}
			}
		}
	}
}