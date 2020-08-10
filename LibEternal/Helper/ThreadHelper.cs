using System;
using System.Reflection;
using System.Threading;

namespace LibEternal.Helper
{
	/// <summary>
	///     An extension class for <see cref="Thread" />s
	/// </summary>
	public static class ThreadHelper
	{
		private static readonly Action<Thread, string> SetThreadName;

		static ThreadHelper()
		{
			//TODO: Should make these method calls faster if possible. Expressions should be fine
			//For thread naming, Unity calls `SetName_internal`. Normal CLR has a private backing field. Check if running in unity by trying to find that method
			MethodInfo setNameMethod = typeof(Thread).GetMethod("SetName_internal", BindingFlags.Static | BindingFlags.NonPublic);
			if (setNameMethod != null)
			{
				//Now we have the method to call to rename the thread, but we still don't have the required `InternalThread` parameter
				PropertyInfo internalThreadProperty = typeof(Thread).GetProperty("Internal", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
				if (internalThreadProperty == null) throw new MissingFieldException("Could not find the InternalThread field.");

				SetThreadName = (thread, newName) =>
				{
					//Get the internal thread
					object internalThread = internalThreadProperty.GetValue(thread);

					//Now set the name using the static method
					setNameMethod.Invoke(null, new[] {internalThread, newName});
				};
			}
			else
			{
				//Try to find the hidden field
				string[] names = {"m_Name", "_name"};
				foreach (string name in names)
				{
					FieldInfo nameField = typeof(Thread).GetField(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
					//Didn't find the backing field, try next name
					if (nameField == null) continue;
					
					//Set our rename delegate.
					//For compatibility (see https://stackoverflow.com/a/18823380), set it to null and then set it to the new name
					SetThreadName = (thread, newName) =>
					{
						//Set it to null
						nameField.SetValue(thread, null);
						//Then set it to what we want
						thread.Name = newName;
					};
					return;
				}

				throw new MissingFieldException($"Could not find System.Threading.Thread's hidden name field. Attempted: {string.Join(", ", names)}");
			}
		}

		/// <summary>
		///     Renames a <see cref="Thread" />, even if it has been named before
		/// </summary>
		/// <param name="thread"></param>
		/// <param name="newName"></param>
		public static void Rename(this Thread thread, string newName)
		{
			SetThreadName(thread, newName);
		}
	}
}