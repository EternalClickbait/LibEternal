using LibEternal.Callbacks.Generic;
using NUnit.Framework;
using System;

namespace LibEternal.Tests.Callbacks
{
	[TestFixture]
	public class SafeActionTests
	{
		[Test]
		public void Ctor_DoesNotThrow()
		{
			Assert.DoesNotThrow(() => _ = new SafeAction());
			Assert.DoesNotThrow(() => _ = new SafeAction(new Action[1]));
			Assert.DoesNotThrow(() => _ = new SafeAction(new Action[] {Ctor_DoesNotThrow}));
		}

		[Test]
		public void EventOperator_AddsToCallbacks()
		{
			SafeAction safeAction = new SafeAction();
			//Shouldn't be any callbacks yet
			Assert.IsEmpty(safeAction.Callbacks);

			//Just some dummy actions
			Action action1 = () => { _ = 1; };
			Action action2 = () => { _ = 2; };
			Action action3 = () => { _ = 3; };

			safeAction.Event += action1;
			Assert.Contains(action1, safeAction.Callbacks);
		}
	}
}