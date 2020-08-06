using LibEternal.Helper;
using LibEternal.JetBrains.Annotations;
using System;
using System.IO;
using UnityEngine;

namespace LibEternal.Unity
{
	/// <summary>
	///     A wrapper for displaying a <see cref="System.IO.FileInfo" /> in a unity inspector. Must use this instead of a standard <see cref="FileInfo" />
	/// </summary>
	[Serializable]
	public class FileInfoWrapper
	{
		/// <summary>
		///     A <see cref="Cached{T}" /> that stores a reference to the <see cref="FileInfo" />. Increases performance by not returning a new
		///     <see cref="FileInfo" /> each time.
		/// </summary>
		[NonSerialized] public Cached<FileInfo> CachedFileInfo;

		/// <summary>
		///     The string representation of the file path.
		/// </summary>
		[HideInInspector] [SerializeField] public string filePath;

		/// <summary>
		///     The default constructor to create a new <see cref="FileInfoWrapper" />
		/// </summary>
		public FileInfoWrapper()
		{
			CachedFileInfo = new Cached<FileInfo>(GetUpdatedFileInfo, null);
		}

		[NotNull]
		private FileInfo GetUpdatedFileInfo()
		{
			return new FileInfo(filePath);
		}

		/// <summary>
		///     Overrides the default <see cref="object.ToString" /> method to return the <see cref="FileInfo" />'s file path instead of the object's type name as a string
		/// </summary>
		/// <returns>
		///     The <see cref="FileInfo" />'s <see cref="FileInfo.FullName" /> property, or an <see cref="string.Empty" /> <see cref="string" /> if it is
		///     <see langword="null" />
		/// </returns>
		[NotNull]
		public override string ToString()
		{
			return CachedFileInfo.Value?.FullName ?? string.Empty;
		}
	}
}