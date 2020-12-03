using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace LibEternal.Unity.Editor
{
	/// <summary>
	///     An editor class for invoking static methods from the unity inspector
	/// </summary>
	[CustomEditor(typeof(StaticMethodInvoker))]
	public sealed class StaticMethodInvokerEditor : UnityEditor.Editor
	{
		/// <summary>
		///     A list of all methods we found
		/// </summary>
		private readonly List<MethodInfo> staticMethods = new List<MethodInfo>(100);

		//We need to somehow store which method we were looking at
		private int namespaceIndex, typeIndex, methodIndex, shortViewMethodIndex;

		//If true, show a large dropdown rather than one each for (the namespace, the class and the method)
		private bool shortView;

		/// <summary>
		///     The default public constructor
		/// </summary>
		public StaticMethodInvokerEditor()
		{
			Init();
		}

		private void Init()
		{
			AssemblyReloadEvents.afterAssemblyReload -= Init;
			AssemblyReloadEvents.afterAssemblyReload += Init;

			//Get a list of all types in the assemblies currently loaded
			var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes());
			
			//Scan for static methods with no parameters
			staticMethods.Clear();
			foreach(Type currentType in types)
			{
				const BindingFlags flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
				var methods = currentType.GetMethods(flags);

				for (int j = 0; j < methods.Length; j++)
				{
					MethodInfo currentMethod = methods[j];
					//Ensure it's static, has no parameters and isn't generic
					if (!currentMethod.IsStatic) continue;
					if (currentMethod.GetParameters().Length != 0) continue;
					if (currentMethod.IsGenericMethod) continue;

					staticMethods.Add(currentMethod);
				}
			}
		}

		/// <inheritdoc />
		public override void OnInspectorGUI()
		{
			shortView = GUILayout.Toggle(shortView, "Use short view");

			if (shortView)
			{
				static string FormatMethodName(MethodInfo methodInfo)
				{
					return methodInfo.DeclaringType?.FullName?.Replace('.', '/').Replace('+', '/') + '/' + methodInfo.Name;
				}

				//Format all the names
				string[] methodNames = staticMethods.Select(FormatMethodName).ToArray();
				shortViewMethodIndex = EditorGUILayout.Popup("Method:", shortViewMethodIndex, methodNames);

				if (GUILayout.Button("Invoke"))
				{
					object returnValue = staticMethods.First(m => FormatMethodName(m) == methodNames[shortViewMethodIndex])
						.Invoke(null, new object[0]);
					Debug.Log($"Return value was: {returnValue ?? "<null>"}");
				}
			}
			else
			{
				//TODO: I really hate this but I don't know of any better way to do it other than LINQ. :(
				//Also this really needs to be optimized a lot

				const string globalNamespaceString = "<Global Namespace>";
				//Get a list of all the namespaces from all the methods. If it is in the global namespace, use the const value instead.
				string[] namespaces = staticMethods.Select(m => m.DeclaringType?.Namespace ?? globalNamespaceString).Distinct().ToArray();
				//Let the user choose which namespace to search
				namespaceIndex = EditorGUILayout.Popup("Namespace:", namespaceIndex, namespaces);

				Type[] types;
				//If the current selected namespace is the global one, the linq won't work
				if (namespaces[namespaceIndex] == globalNamespaceString)
				{
					//Get all the methods where their class's namespace is null
					IEnumerable<MethodInfo> methodTypes = staticMethods.Where(m => m.DeclaringType?.Namespace == null);
					//And get their declaring class
					types = methodTypes.Select(m => m.DeclaringType).Distinct().ToArray();
				}
				else
				{
					//Same as above, just comparing to the selected namespace not null
					types = staticMethods.Where(m => m.DeclaringType?.Namespace == namespaces[namespaceIndex])
						.Select(m => m.DeclaringType).Distinct().ToArray();
				}

				if (types.Length != 0)
					typeIndex = EditorGUILayout.Popup("Type:", typeIndex, types.Select(t => t.Name).ToArray());
				else
					EditorGUILayout.Popup("Type:", 0, new[] {"<No Types Found>"});

				string[] methodNames;
				// ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
				if (types.Length != 0)
					methodNames = staticMethods.Where(m => types[typeIndex] == m.DeclaringType).Select(m => m.Name).Distinct().ToArray();
				else methodNames = new[] {"<No Methods Found>"};

				methodIndex = EditorGUILayout.Popup("Method:", methodIndex, methodNames);

				//Show a button to invoke the method
				if (GUILayout.Button("Invoke"))
				{
					object returnValue = staticMethods.First(m =>
						m.DeclaringType == types[typeIndex]
						&& m.Name == methodNames[methodIndex]
					).Invoke(null, new object[0]);
					Debug.Log($"Return value was: {returnValue ?? "<null>"}");
				}
			}

			GUILayout.Space(20);
			// ReSharper disable once InvertIf
			if (GUILayout.Button("Update list of functions", EditorStyles.miniButtonMid))
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				Init();
				stopwatch.Stop();

				Debug.Log($"Updating function list took {stopwatch.Elapsed.TotalMilliseconds:n2}ms");
			}
		}
	}
}