using LibEternal.Extensions;
using LibEternal.Helper;
using LibEternal.JetBrains.Annotations;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LibEternal.Unity.Editor
{
	[CustomPropertyDrawer(typeof(FileInfoWrapper), true)]
	public class FileInfoWrapperDrawer : PropertyDrawer
	{
		/// <summary>
		///     The texture used for the button used to select a file
		/// </summary>
		private static readonly Texture2D FolderButtonTex = GetFolderButtonTexture();

		/// <summary>
		///     A <see cref="GUIStyle" /> used when displaying the full path of the <see cref="FileInfo" />.
		///     I tried to make this mimic when the gui is disabled, because changing it has no effect
		/// </summary>
		private static readonly GUIStyle FullPathGuiStyle = new GUIStyle(EditorStyles.textField)
		{
			//Align it to the right so the file name is visible without having to select it
			alignment = TextAnchor.MiddleRight,

			richText = true, normal =
			{
				textColor = Color.gray
			},
			focused =
			{
				textColor = Color.grey
			},
			hover =
			{
				textColor = new Color(0.2f, 0.2f, 0.2f)
			}
		};

		//This doesn't work at the moment, but it's here for future compatibility
		[NotNull]
		private static Texture2D GetFolderButtonTexture()
		{
			Texture2D tex = new Texture2D(10, 10);

			for (int x = 0; x < tex.width; x++)
			{
				for (int y = 0; y < tex.height; y++)
					// tex.SetPixel(x, y, Color.green);
					tex.SetPixel(x, y, Random.ColorHSV(0, 1, .8f, 1, 0, 1, 1, 1));
			}

			return tex;
		}

		public override void OnGUI(Rect position, [NotNull] SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			int oldIndent = EditorGUI.indentLevel;
			//Prevents this from being double-indented
			EditorGUI.indentLevel = 0;

			//This should take up most of the available space, leaving 20 pixels for the select file button
			Rect fileNameRect = new Rect(position.x, position.y, position.width - 20, position.height / 2);
			Rect folderSelectButtonRect = new Rect(fileNameRect.xMax, position.y, 20, fileNameRect.height); //Should take up the rest of the 'line'
			Rect fullPathRet = new Rect(position.x, fileNameRect.yMax, position.width, position.height / 2);

			//FINALLY!!! I love that I don't have to manually update this each time something changes
			FileInfoWrapper targetWrapper = property.GetSelectedFromPath<FileInfoWrapper>();

			//Get the file name
			string displayedFileName;
			Cached<FileInfo> cachedFileInfo = targetWrapper.CachedFileInfo;

			//If the string path is null, we'll get a lot of exceptions, so create a separate path
			if (string.IsNullOrEmpty(targetWrapper.filePath))
				displayedFileName = "<No file selected>";
			else if (!IoExtensions.IsValidFilename(targetWrapper.filePath))
				displayedFileName = "<Invalid file path>";
			else
				try
				{
					FileInfo info = cachedFileInfo.Value;
					displayedFileName = info.Name; //This should give the short name e.g. 'Test.cs'
				}
				catch
				{
					displayedFileName = "<Error Getting Name>";
				}

			//Show the name
			EditorGUI.LabelField(fileNameRect, displayedFileName);

			//Draw a button that opens a file selection dialogue
			if (GUI.Button(folderSelectButtonRect, FolderButtonTex))
			{
				string selectedPath = EditorUtility.OpenFilePanel("Select a file", targetWrapper.filePath ?? "", "");

				//Save the file path and update it
				targetWrapper.filePath = selectedPath;
				targetWrapper.CachedFileInfo.UpdateValue();
			}

			//This allows the user to set the file using the path string
			string newPath = EditorGUI.DelayedTextField(fullPathRet, targetWrapper.filePath, FullPathGuiStyle);
			if (targetWrapper.filePath != newPath)
				//Prevents the user from setting an invalid path
				try
				{
					//Converts it to the OS's path format
					newPath = new FileInfo(newPath).FullName;

					targetWrapper.filePath = newPath;
					targetWrapper.CachedFileInfo.UpdateValue();
				}
				catch
				{
					//The user entered an invalid path, but nothing is changed so ignore the exception
				}

			EditorGUI.indentLevel = oldIndent;
			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			//Tell the inspector this property uses 2 lines
			return base.GetPropertyHeight(property, label) * 2;
		}
	}
}