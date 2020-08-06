using LibEternal.JetBrains.Annotations;
using System;

namespace LibEternal.Abstraction
{
	/// <summary>
	///     An abstract class that provides a single method (<see cref="DisposeInherited" />) that should be overriden to dispose of any objects. Calling of the function will be handled automatically
	/// </summary>
	[PublicAPI]
	public abstract class SmartDisposable : IDisposable
	{
		/// <summary>
		///     If this object has already been disposed
		/// </summary>
		protected bool Disposed = false;

		/// <summary>
		///     Disposes of the <see cref="SmartDisposable" />. Note that if the object has already been disposed, calling this method will do nothing
		/// </summary>
		public void Dispose()
		{
			// Check to see if Dispose has already been called.
			if (Disposed) return;

			// Note disposing has been done. Set this before we've actually disposed to prevent some errors.
			Disposed = true;

			//Call the inherited function, which should dispose of any other objects
			DisposeInherited();

			//This object was disposed by manually calling Dispose(), so to prevent the dispose code being called twice, we suppress the finalizer
			//As a bonus this saves some performance :)
			GC.SuppressFinalize(this);
		}

		/// <summary>
		///     Throws an <see cref="ObjectDisposedException" /> if the object has already been <see cref="Disposed" />.
		/// </summary>
		/// <exception cref="ObjectDisposedException">The exception thrown if the object is disposed</exception>
		protected void ThrowIfDisposed()
		{
			if (Disposed) throw new ObjectDisposedException(ToString(), $"Cannot access a disposed {GetType().FullName}");
		}

		/// <summary>
		///     The finalizer called when the <see cref="GC" /> wants do collect this object. Simply calls the <see cref="Dispose" /> method
		/// </summary>
		~SmartDisposable()
		{
			//Don't need to do dispose checking here, it's already implemented in Dispose()
			//Use the proper dispose method
			Dispose();
		}

		/// <summary>
		///     This override should dispose of any objects the inheritor has created.
		/// </summary>
		protected abstract void DisposeInherited();
	}
}