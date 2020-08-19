using LibEternal.Callbacks.Generic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace LibEternal.Tests.Callbacks
{
	//We don't need to create a separate test class for each SafeFunc generic,
	//because they are all created using the same template, so the code should be identical
	[TestFixture]
	public class SafeFuncTests
	{
		[Test]
		public void Ctor_DoesNotThrow()
		{
			Assert.DoesNotThrow(() => _ = new SafeFunc<int>());
			Assert.DoesNotThrow(() => _ = new SafeFunc<int>(new Func<int>[1]));
			Assert.DoesNotThrow(() => _ = new SafeFunc<int>(new Func<int>[] {() => 1}));
		}

		[Test]
		public void EventOperator_AddsOrRemovesCallbacks()
		{
			SafeFunc<int> safeFunc = new SafeFunc<int>();
			//Shouldn't be any callbacks yet
			CollectionAssert.IsEmpty(safeFunc.Callbacks, "Callbacks is not empty when no objects added");

			//Checking it was added
			safeFunc.Event += DummyAction;
			CollectionAssert.Contains(safeFunc.Callbacks, (Func<int>) DummyAction, "Action not in invocation list, even though it was added");
			//Checking it was removed
			safeFunc.Event -= DummyAction;
			CollectionAssert.DoesNotContain(safeFunc.Callbacks, (Func<int>) DummyAction, "Action in invocation list after being removed");

			//Should be empty now; everything that was added was removed 
			CollectionAssert.IsEmpty(safeFunc.Callbacks, "Callbacks not empty when all actions removed");
		}

		[ExcludeFromCodeCoverage]
		private static int DummyAction()
		{
			return default;
		}

		[Test]
		public void InvokeSafe_SkipsNullCallbacks()
		{
			SafeFunc<int> safeFunc = new SafeFunc<int>();
			safeFunc.Event += null;

			Assert.DoesNotThrow(() => safeFunc.InvokeSafe());
			Assert.IsEmpty(safeFunc.InvokeSafe().Exceptions);
		}

		[Test]
		public void InvokeSafe_CatchesAndReturnsExceptions()
		{
			const int returnVal = 0x12345678;
			Exception exception = new Exception("Dummy Exception");
			//One function should throw, the other should do return default
			// ReSharper disable once IdentifierTypo
			Func<int>[] funcs = {() => throw exception, () => returnVal};

			SafeFunc<int> safeFunc = new SafeFunc<int>(funcs);
			(List<Exception> exceptions, List<int> results) = safeFunc.InvokeSafe();

			//We should have a list of exceptions, containing one element
			Assert.AreEqual(1, exceptions.Count, "Number of caught exceptions is incorrect");
			//Assert that the exception caught was the correct one
			Assert.AreEqual(exception, exceptions[0], "The caught exception is incorrect");

			//Do the same for the return result
			Assert.AreEqual(1, results.Count, "Number of returned results is incorrect");
			//Assert that the exception caught was the correct one
			Assert.AreEqual(returnVal, results[0], "The returned result is incorrect");
		}
	}
}