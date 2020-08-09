using System.Reflection;
using System.Threading;

namespace LibEternal.Helper
{
	/// <summary>
	/// An extension class for <see cref="Thread"/>s
	/// </summary>
	public static class ThreadHelper
	{
		/// <summary>
		/// Represents a <see cref="Thread"/>'s hidden name property
		/// </summary>
		private static readonly FieldInfo ThreadNameField = FindThreadNameField();
		
		/// <summary>
		/// Renames a <see cref="Thread"/>, even if it has been named before
		/// </summary>
		/// <param name="thread"></param>
		/// <param name="newName"></param>
		public static void Rename(this Thread thread, string newName)
		{
			//IDK Why, but this sometimes won't work when using Unity, so do nothing instead of getting a NullReferenceException
			if (ThreadNameField is null) return;
			//For compatibility (see https://stackoverflow.com/a/18823380), set it to null and then set it to the new name
			ThreadNameField.SetValue(thread, null);
			thread.Name = newName;
		}

		private static FieldInfo FindThreadNameField()
		{
			string[] names = {"m_Name", "_name"};
			foreach (string name in names)
			{
				FieldInfo field = typeof(Thread).GetField(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
				if (field != null) return field;
			}
			return null;
			// throw new MissingFieldException($"Could not find System.Threading.Thread's hidden name field. Attempted: {string.Join(", ", names)}");
		}
	}
}