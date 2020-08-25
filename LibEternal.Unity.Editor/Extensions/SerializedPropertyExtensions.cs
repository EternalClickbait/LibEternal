using LibEternal.JetBrains.Annotations;
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;

namespace LibEternal.Unity.Editor
{
	/// <summary>
	///     A class of extensions for a <see cref="SerializedProperty" />
	/// </summary>
	[PublicAPI]
	public static class SerializedPropertyExtensions
	{
		/// <summary>
		///     Array paths are in the format object.Array.data[index]. First match group is the (field) name of the array, second is the index
		/// </summary>
		private static readonly Regex ArrayRegex = new Regex(@"^(\w+)(?:\.Array\.data\[)(\d+)(?:\])", RegexOptions.Compiled);

		/// <summary>
		///     Searches a <paramref name="property" />'s path for the selected object of type <typeparamref name="T" />, and returns it.
		/// </summary>
		/// <param name="property"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T GetSelectedFromPath<T>([NotNull] this SerializedProperty property)
		{
			//The maximum depth allowed for an object. Objects deeper than this will throw an exception
			//This is mainly here to avoid an infinite loop because of the while loop
			const int maxObjectDepth = 10;

			//Get the full path to begin with
			string path = property.propertyPath;

			object targetObject = property.serializedObject.targetObject;
			int iterations = 0;

			while (true)
			{
				iterations++;
				Match arrayMatch = ArrayRegex.Match(path);

				//If we have a match for an array, parse it
				if (arrayMatch.Success)
				{
					//See array regex definition for group indices
					string fieldName = arrayMatch.Groups[1].Value;
					string indexString = arrayMatch.Groups[2].Value;

					//Gets the field of the array we want to index
					Type targetObjectClassType = targetObject.GetType();
					FieldInfo arrayField = targetObjectClassType.GetField(fieldName);

					var array = (object[]) arrayField.GetValue(targetObject);
					int index = int.Parse(indexString);

					//Match.Length returns the amount of chars matched
					//path.Remove() returns a new string, with all the characters between 0 and match.Length removed. Should essentially delete the matched section (it should be at the start)
					path = path.Remove(0, arrayMatch.Length);

					targetObject = array[index];
				}
				else //The next object is a field
				{
					string fieldName;
					//If the path doesn't contain a '.', we're on the 2nd last object (so this should assign the final field)
					//Without this check, Path.IndexOf() returns -1, which then makes Substring() fail
					// ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
					if (!path.Contains("."))
						fieldName = path;
					else
						//Substrings up to the first '.'
						fieldName = path.Substring(0, path.IndexOf('.'));

					//Removes the parsed section. Same concept as in the array parser
					path = path.Remove(0, fieldName.Length);

					//Gets the field of the object we want to grab
					Type targetObjectClassType = targetObject.GetType();
					FieldInfo fieldInfo = targetObjectClassType.GetField(fieldName);

					targetObject = fieldInfo.GetValue(targetObject);
				}

				//First, check if we've completely parsed the object, to avoid errors further down
				if (path.Length == 0)
					//We've parsed the entire path, so the target object is the one we want to return
					return (T) targetObject;

				//We need to remove the leading '.' from the string
				if (path[0] == '.') path = path.Remove(0, 1);

				//Allow a maximum of 10 iterations. Objects deeper than 10 levels will throw.
				if (iterations > maxObjectDepth) throw new Exception($"Took more than {maxObjectDepth} iterations");
			}
		}
	}
}