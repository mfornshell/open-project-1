using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UOP1.StateMachine;

namespace UOP1.Tests
{
	public class StateTransitionTests
	{

		[Test]
		public void TryGetTransition_WithTrueConditions_ReturnsTrue()
		{
			var state = new State();
			var conditions = Enumerable.Repeat(new StateCondition(null, new MockCondition(true), true), 10);

			//resultGroups should get a value, once i figure out what it is supposed to look like
			var transition = new StateTransition(state, conditions.ToArray(), null);

			var actual = transition.TryGetTransiton(out var s);

			Assert.True(actual);
			Assert.IsNotNull(s);
		}

		[Test]
		public void TryGetTransition_WithAnyTrueGroup_ReturnsTrue()
		{
			var state = new State();
			var conditions = Enumerable.Range(0, 10).Select(
				x => new StateCondition(null, new MockCondition(x < 5), true));

			var groups = new int[] { 5, 5 };
			var transition = new StateTransition(state, conditions.ToArray(), groups);

			var actual = transition.TryGetTransiton(out var s);

			Assert.True(actual);
			Assert.IsNotNull(s);
		}

		[Test]
		public void TryGetTransition_WithAllFalseGroups_ReturnsFalse()
		{
			var state = new State();
			var conditions = Enumerable.Range(0, 10).Select(
				x => new StateCondition(null, new MockCondition(x % 2 == 0), true));

			var groups = new int[] { 5, 5 };
			var transition = new StateTransition(state, conditions.ToArray(), groups);

			var actual = transition.TryGetTransiton(out var s);

			Assert.False(actual);
			Assert.Null(s);
		}
	}
}
