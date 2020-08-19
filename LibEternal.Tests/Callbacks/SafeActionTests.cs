using LibEternal.Callbacks.Generic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace LibEternal.Tests.Callbacks
{
	//We don't need to create a separate test class for each SafeAction generic,
	//because they are all created using the same template, so the code should be identical
	[TestFixture]
	public class SafeActionTests
	{
		[Test]
		public void InvokeSafe_SkipsNullCallbacks()
		{
			SafeAction safeAction = new SafeAction();
			safeAction.Event += null;

			Assert.DoesNotThrow(() => safeAction.InvokeSafe());
		}

		[Test]
		public void Ctor_DoesNotThrow()
		{
			Assert.DoesNotThrow(() => _ = new SafeAction());
			Assert.DoesNotThrow(() => _ = new SafeAction(new Action[1]));
			Assert.DoesNotThrow(() => _ = new SafeAction(new Action[] {Ctor_DoesNotThrow}));
		}

		[Test]
		public void EventOperator_AddsOrRemovesCallbacks()
		{
			SafeAction safeAction = new SafeAction();
			//Shouldn't be any callbacks yet
			CollectionAssert.IsEmpty(safeAction.Callbacks, "Callbacks is not empty when no objects added");

			//Checking it was added
			safeAction.Event += DummyAction;
			CollectionAssert.Contains(safeAction.Callbacks, (Action) DummyAction, "Action not in invocation list, even though it was added");
			//Checking it was removed
			safeAction.Event -= DummyAction;
			CollectionAssert.DoesNotContain(safeAction.Callbacks, (Action) DummyAction, "Action in invocation list after being removed");

			//Should be empty now; everything that was added was removed 
			CollectionAssert.IsEmpty(safeAction.Callbacks, "Callbacks not empty when all actions removed");
		}

		[ExcludeFromCodeCoverage]
		private static void DummyAction()
		{
		}

		[Test]
		public void InvokeSafe_CatchesAndReturnsExceptions()
		{
			Exception exception = new Exception("Dummy Exception");
			//One action should throw, the other should do nothing
			Action[] actions = {() => throw exception, () => { }};

			SafeAction safeAction = new SafeAction(actions);
			List<Exception> exceptions = safeAction.InvokeSafe();
			//We should have a list of exceptions, containing one element
			Assert.AreEqual(1, exceptions.Count, "Number of caught exceptions is incorrect");
			//Assert that the exception caught was the correct one
			Assert.AreEqual(exception, exceptions[0], "The caught exception is not the correct one");
		}
	}
}