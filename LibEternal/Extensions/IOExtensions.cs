using LibEternal.JetBrains.Annotations;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace LibEternal.Extensions
{
	/// <summary>
	///     An extension class for <see cref="System.IO" /> related operations
	/// </summary>
	[PublicAPI]
	public class IoExtensions
	{
		private static readonly Regex InvalidFilePathChars =
			new Regex($"[{Regex.Escape(new string(Path.GetInvalidPathChars()))}]", RegexOptions.Compiled);

		/// <summary>
		///     Returns <see langword="true" /> if the supplied string is a valid file name
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		[Pure]
		public static bool IsValidFilename([CanBeNull] string fileName)
		{
			if (string.IsNullOrWhiteSpace(fileName)) return false;
			//Check file length (https://docs.microsoft.com/en-us/windows/win32/fileio/naming-a-file#maximum-path-length-limitation)
			if (fileName.Length > 260) return false;
			//From https://stackoverflow.com/a/62855/
			return !InvalidFilePathChars.IsMatch(fileName);
		}
	}
}