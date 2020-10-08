using LibEternal.JetBrains.Annotations;
using System;

namespace LibEternal.Exceptions
{
	/// <summary>
	/// A class that represents a fatal exception. If a <see cref="FatalException"/> is thrown, it is assumed that a critical error has occured, and therefore the application should close immediately
	/// </summary>
	[PublicAPI]
	public class FatalException : Exception
	{
		
	}
}