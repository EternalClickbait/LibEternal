using LibEternal.Abstraction;
using NUnit.Framework;
using System;

namespace LibEternal.Tests.Abstraction
{
	[TestFixture]
	public class SmartDisposableTests
	{
		[Test]
		public void Dispose_SetsDisposeProperty_ToTrue()
		{
			Disposable disposable = new Disposable();
			disposable.Dispose();
			Assert.True(disposable.Disposed);
		}

		[Test]
		public void ThrowIfDisposed_WhenDisposed_ThrowsObjectDisposedException()
		{
			Disposable disposable = new Disposable();
			disposable.Dispose();
			Assert.Throws<ObjectDisposedException>(disposable.ThrowIfDisposed);
		}

		[Test]
		public void ThrowIfDisposed_WhenNotDisposed_ThrowsObjectDisposedException()
		{
			Disposable disposable = new Disposable();
			Assert.DoesNotThrow(disposable.ThrowIfDisposed);
		}

		[Test]
		public void MultipleDisposeCalls_DoesNothing()
		{
			//Dispose a few times
			using (Disposable disposable = new Disposable())
			{
				disposable.Dispose();
				disposable.Dispose();
			}
		}

		[Test]
		public void GcCollect_CallsDispose()
		{
			WeakReference<Disposable> weakRef;
			bool disposeCalled = false;

			void CreateDisposableGarbage()
			{
				// This will go out of scope after this function is executed. Otherwise, it won't be collected
				Disposable disposable = new Disposable {OnDisposeInherited = () => disposeCalled = true};
				weakRef = new WeakReference<Disposable>(disposable, false);
			}

			// Create the garbage, then collect all objects
			CreateDisposableGarbage();
			GC.Collect(0, GCCollectionMode.Forced);
			GC.WaitForPendingFinalizers();

			//Check if the object was finalized
			Assert.False(weakRef.TryGetTarget(out Disposable disposable), "WeakReference still contains valid target");
			Assert.Null(disposable, "WeakReference target is not null");
			//Ensure dispose was actually called
			Assert.True(disposeCalled, "Dispose was not called");
		}

		private sealed class Disposable : SmartDisposable
		{
			public Action OnDisposeInherited;

			protected override void DisposeInherited()
			{
				OnDisposeInherited?.Invoke();
			}
		}
	}
}