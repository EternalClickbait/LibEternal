using LibEternal.JetBrains.Annotations;
using System;
using System.Runtime.Serialization;

namespace LibEternal.Exceptions
{
	/// <summary>
	///     A type of <see cref="Exception" /> that is thrown whenever an attempt is made to execute an invalid code path.
	/// </summary>
	[PublicAPI]
	[Serializable]
	public class InvalidCodePathException : Exception
	{
		/// <inheritdoc />
		public InvalidCodePathException()
		{
		}

		/// <inheritdoc />
		public InvalidCodePathException(string message) : base(message)
		{
		}

		/// <inheritdoc />
		public InvalidCodePathException(string message, Exception inner) : base(message, inner)
		{
		}

		/// <inheritdoc />
		protected InvalidCodePathException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}
	}
}