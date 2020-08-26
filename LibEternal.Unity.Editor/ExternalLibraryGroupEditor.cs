using LibEternal.Unity.Editor.Extensions;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using static LibEternal.Extensions.IoExtensions;
using Debug = UnityEngine.Debug;

// ReSharper disable RedundantLogicalConditionalExpressionOperand

// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable HeuristicUnreachableCode
#pragma warning disable 162

namespace LibEternal.Unity.Editor
{
	/// <summary>
	///     An editor class for an <see cref="ExternalLibraryGroup" />
	/// </summary>
	[CustomEditor(typeof(ExternalLibraryGroup))]
	public sealed class ExternalLibraryGroupEditor : UnityEditor.Editor
	{
		/// <summary>
		///     Set this to true to print extended information such as when a library is being copied/imported
		/// </summary>
		private const bool PrintExtendedInfo = false;

		/// <summary>
		///     Set this to true to not print any messages at all
		/// </summary>
		private const bool Silent = false;

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

		/// <summary>
		///     Compiles and imports a single <see cref="ExternalLibraryGroup" />
		/// </summary>
		/// <param name="libraryGroup">The <see cref="ExternalLibraryGroup" /> to compile and import</param>
		/// <returns></returns>
		// ReSharper disable once MemberCanBePrivate.Global
		public static async Task Recompile(ExternalLibraryGroup libraryGroup)
		{
			if (libraryGroup.solutions is null || libraryGroup.solutions.Length == 0)
			{
				if (!Silent)
					Debug.LogWarning(
						$"\tWarning: No solutions to compile for group {libraryGroup.name}. Did you forget to set them in the inspector?");
			}
			else
			{
				if (PrintExtendedInfo && !Silent)
					Debug.Log($"\tCompiling library group {libraryGroup.name}");
				//Loop over all the libraries and compile them
				for (int i = 0; i < libraryGroup.solutions.Length; i++)
				{
					FileInfoWrapper solution = libraryGroup.solutions[i];
					// ReSharper disable once RedundantAssignment
					Stopwatch stopwatch = Stopwatch.StartNew();

					FileInfo fileInfo;
					try
					{
						fileInfo = solution.CachedFileInfo.UpdateAndGetValue();
					}
					//If there was an error getting the fileInfo, warn the user
					catch (Exception e)
					{
						if (!Silent)

							Debug.LogWarning($"\t\tWarning: Invalid path for solution at index [{i}] (\"{solution.filePath}\"):\n{e}");
						continue;
					}

					if (PrintExtendedInfo && !Silent)
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
								//If for some reason msbuild doesn't work, use 'dotnet build' instead. '-nologo' just reduces the excess output. '--verbosity q' makes it only print the warning/errors
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
							process.Kill();
							if (!Silent)
								Debug.LogError(
									$"\t\tError: Force Killed Process - build took too long ({stopwatch.Elapsed:m\\:ss}). Output was:\n{await process.StandardOutput.ReadToEndAsync()}");
							process.Dispose();
						}
						else
						{
							//Trim the newlines so the user can see if the build failed without expanding the debug message
							string trimmedOutput = (await process.StandardOutput.ReadToEndAsync()).TrimStart('\r', '\n');
							bool buildFailed = trimmedOutput.Contains("Build FAILED");
							//This formats the elapsed time as <minutes (short)> minutes and <seconds (long)>.<decimal seconds (short> seconds
							//e.g. 0min 5.325s => "0 minutes and 5.32 seconds"
							if (!Silent)
								if (buildFailed)
									Debug.LogError($"\t\tBuilt {fileInfo.Name} in {stopwatch.Elapsed:m\\:ss}. Output was: {trimmedOutput}");
								else
									Debug.Log($"\t\tBuilt {fileInfo.Name} in {stopwatch.Elapsed:m\\:ss}. Output was: {trimmedOutput}");
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
				if (!Silent)
					Debug.LogWarning(
						$"\tWarning: No libraries to import for group {libraryGroup.name}. Did you forget to set them in the inspector?");
			}
			else
			{
				if (PrintExtendedInfo && !Silent)
					Debug.Log($"\tImporting library group {libraryGroup.name}");
				for (int i = 0; i < libraryGroup.libraries.Length; i++)
				{
					ExternalLibraryGroup.CompiledLibrary compiledLibrary = libraryGroup.libraries[i];

					//Checks to ensure nothing errors out
					if (!IsValidFilename(compiledLibrary.sourceLocation.filePath))
					{
						if (!Silent)
							Debug.LogWarning($"\t\tWarning: Invalid source file selected for library at index [{i}] ");
						continue;
					}

					if (!IsValidFilename(compiledLibrary.assetDestination.filePath))
					{
						Debug.LogWarning($"\t\tWarning: Invalid destination file selected for library at index [{i}] ");
						continue;
					}

					if (PrintExtendedInfo && !Silent)
						Debug.Log(
							$"\t\tCopying binary from {compiledLibrary.sourceLocation.filePath} to {compiledLibrary.assetDestination.filePath}");
					File.Copy(compiledLibrary.sourceLocation.filePath, compiledLibrary.assetDestination.filePath, true);

					//Can't import a group at a time, so import as we go
					AssetDatabase.ImportAsset(compiledLibrary.assetDestination.filePath,
						ImportAssetOptions.ForceUpdate | ImportAssetOptions.DontDownloadFromCacheServer);
					if (!Silent)
						Debug.Log($"\t\tImported binary \"{compiledLibrary.assetDestination.CachedFileInfo.Value.Name}\"");
				}

				if (PrintExtendedInfo && !Silent)
					Debug.Log($"\tFinished importing library group {libraryGroup.name}");
			}
		}

		/// <summary>
		///     Compiles all external libraries and imports them
		/// </summary>
		[MenuItem("Assets/Recompile External Libraries")]
		public static async void RecompileAll()
		{
			ExternalLibraryGroup[] allInstances = ScriptableObjectExtensions.GetAllInstances<ExternalLibraryGroup>();
			if (allInstances is null || allInstances.Length == 0) return;

			//Loop over all groups
			foreach (ExternalLibraryGroup libraryGroup in allInstances)
			{
				//Sometimes I was getting NullReferenceExceptions, this fixed it
				if (libraryGroup is null) continue;

				await Recompile(libraryGroup);
			}

			if (!Silent)
				Debug.Log("Finished compiling all library groups");
		}
	}
}