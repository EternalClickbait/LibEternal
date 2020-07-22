using LibEternal.JetBrains.Annotations;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using static LibEternal.Extensions.IoExtensions;
using Debug = UnityEngine.Debug;

// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable HeuristicUnreachableCode
#pragma warning disable 162

namespace LibEternal.Unity.Editor
{
	/// <summary>
	/// An editor class for an <see cref="ExternalLibraryGroup"/>
	/// </summary>
	[CustomEditor(typeof(ExternalLibraryGroup))]
	public sealed class ExternalLibraryGroupEditor : UnityEditor.Editor
	{
		/// <summary>
		///     Set this to true to print extended information such as when a library is being copied/imported
		/// </summary>
		private const bool PrintExtendedInfo = false;

		/// <inheritdoc />
		public override void OnInspectorGUI()
		{
			//This draws the FileInfoWrappers using the property drawer
			base.OnInspectorGUI();

			if (GUILayout.Button("Recompile all"))
				RecompileAll();

			//This fixes a problem where the object doesn't get saved after a editor restart
			EditorUtility.SetDirty(target);
		}

		//TODO: Should make a function to only do this for 1 instance
		/// <summary>
		///     Compiles all external libraries and imports them
		/// </summary>
		[DidReloadScripts]
		[MenuItem("Assets/Recompile External Libraries")]
		public static async void RecompileAll()
		{
			Debug.Log("Recompiling all external libraries");

			ExternalLibraryGroup[] allInstances = GetAllInstances();
			if (allInstances is null || allInstances.Length == 0)
			{
				Debug.LogWarning("No library groups found");
				return;
			}

			//Loop over all groups
			foreach (ExternalLibraryGroup libraryGroup in allInstances)
			{
				//Sometimes I was getting NullReferenceExceptions, this fixed it
				if (libraryGroup is null) continue;

				if (libraryGroup.solutions is null || libraryGroup.solutions.Length == 0)
				{
					Debug.LogWarning(
						$"\tWarning: No solutions to compile for group {libraryGroup.name}. Did you forget to set them in the inspector?");
				}
				else
				{
					if (PrintExtendedInfo)
						Debug.Log($"\tCompiling library group {libraryGroup.name}");
					//Loop over all the libraries and compile them
					for (int i = 0; i < libraryGroup.solutions.Length; i++)
					{
						FileInfoWrapper solution = libraryGroup.solutions[i];
						Stopwatch stopwatch = Stopwatch.StartNew();

						FileInfo fileInfo;
						try
						{
							fileInfo = solution.CachedFileInfo.UpdateAndGetValue();
						}
						//If there was an error getting the fileInfo, warn the user
						catch (Exception e)
						{
							Debug.LogWarning($"\t\tWarning: Invalid path for solution at index [{i}] (\"{solution.filePath}\"):\n{e}");
							continue;
						}

						if (PrintExtendedInfo)
							Debug.Log($"\t\tCompiling solution '{fileInfo.Name}' (at '{fileInfo.FullName}')");

						try
						{
							Process process = new Process
							{
								StartInfo =
								{
									//Adding the '-m' switch builds the projects in the solution in parallel
									// FileName = "msbuild", Arguments = $"\"{library.filePath}\" -m",

									// ReSharper disable once CommentTypo
									//If for some reason msbuild doesn't work, use 'dotnet build' instead. '/nologo' just reduces the excess output. '--verbosity q' makes it only print the warning/errors
									FileName = "dotnet", Arguments = $"build \"{fileInfo.FullName}\" --verbosity q -nologo",

									//These let us read the process output as a stream
									RedirectStandardOutput = true, UseShellExecute = false, RedirectStandardError = true,
									//Don't create an output window (no command prompt popping up)
									CreateNoWindow = true
								}
							};
							process.Start();
							//Give it 30 sec to build
							Task delayTask = Task.Delay(TimeSpan.FromSeconds(30));
							Task buildTask = new Task(process.WaitForExit);
							buildTask.Start();

							//Returns whichever completes first
							Task first = await Task.WhenAny(buildTask, delayTask);

							//Force kill if it took too long (the delay task returned first)
							if (first != buildTask)
							{
								Debug.LogError($"\t\tError: Build took too long ({stopwatch.Elapsed:m\\:ss}), force killing");
								process.Kill();
								process.Dispose();
							}
							else
							{
								//Trim the newlines so the user can see if the build failed without expanding the debug message
								string output = process.StandardOutput.ReadToEnd().TrimStart('\r', '\n');
								//This formats the elapsed time as <minutes (short)> minutes and <seconds (long)>.<decimal seconds (short> seconds
								//e.g. 0min 5.325s => "0 minutes and 5.32 seconds"
								Debug.Log($"\t\tBuilt {fileInfo.Name} in {stopwatch.Elapsed:m\\:ss}. Output was: {output}");
								process.Dispose();
							}
						}
						//Log any exceptions
						catch (Exception e)
						{
							Debug.LogException(e);
						}
					}
				}

				if (libraryGroup.libraries is null || libraryGroup.libraries.Length == 0)
				{
					Debug.LogWarning(
						$"\tWarning: No libraries to import for group {libraryGroup.name}. Did you forget to set them in the inspector?");
				}
				else
				{
					if (PrintExtendedInfo)
						Debug.Log($"\tImporting library group {libraryGroup.name}");
					for (int i = 0; i < libraryGroup.libraries.Length; i++)
					{
						ExternalLibraryGroup.CompiledLibrary compiledLibrary = libraryGroup.libraries[i];
			
						//Checks to ensure nothing errors out
						if (!IsValidFilename(compiledLibrary.sourceLocation.filePath))
						{
							Debug.LogWarning($"\t\tWarning: Invalid source file selected for library at index [{i}] ");
							continue;
						}

						if (!IsValidFilename(compiledLibrary.assetDestination.filePath))
						{
							Debug.LogWarning($"\t\tWarning: Invalid destination file selected for library at index [{i}] ");
							continue;
						}

						if (PrintExtendedInfo)
							Debug.Log(
								$"\t\tCopying binary from {compiledLibrary.sourceLocation.filePath} to {compiledLibrary.assetDestination.filePath}");
						File.Copy(compiledLibrary.sourceLocation.filePath, compiledLibrary.assetDestination.filePath, true);

						//Can't import a group at a time, so import as we go
						AssetDatabase.ImportAsset(compiledLibrary.assetDestination.filePath,
							ImportAssetOptions.ForceUpdate | ImportAssetOptions.DontDownloadFromCacheServer);
						Debug.Log($"\t\tImported binary \"{compiledLibrary.assetDestination.CachedFileInfo.Value.Name}\"");
					}

					if (PrintExtendedInfo)
						Debug.Log($"\tFinished importing library group {libraryGroup.name}");
				}
			}

			Debug.Log("Finished compiling all library groups");
		}

		/// <summary>
		///     Gets an array of all the external library compiler assets
		/// </summary>
		/// <returns></returns>
		// ReSharper disable once ReturnTypeCanBeEnumerable.Local
		[NotNull]
		private static ExternalLibraryGroup[] GetAllInstances()
		{
			string[]
				guids = AssetDatabase.FindAssets("t:" + typeof(ExternalLibraryGroup).Name); //FindAssets uses tags, check documentation for more info
			ExternalLibraryGroup[] array = new ExternalLibraryGroup[guids.Length];
			for (int i = 0; i < guids.Length; i++) //probably could get optimized 
			{
				string path = AssetDatabase.GUIDToAssetPath(guids[i]);
				array[i] = AssetDatabase.LoadAssetAtPath<ExternalLibraryGroup>(path);
			}

			return array;
		}
	}
}