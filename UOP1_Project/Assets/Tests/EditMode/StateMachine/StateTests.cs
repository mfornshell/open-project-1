using NUnit.Framework;
using System.Linq;
using UOP1.StateMachine;

namespace UOP1.Tests
{
	public class StateTests
	{
		[Test]
		public void TryGetTransition_CanTransition_ReturnsTrueWithValidState()
		{
			var transitions = Enumerable.Repeat(StateTransitionHelpers.Create(), 5);

			var state = new State(null, null, transitions.ToArray(), null);

			var actual = state.TryGetTransition(out var s);

			Assert.True(actual);
			Assert.NotNull(s);
		}

		[Test]
		public void TryGetTransition_CanNotTransition_ReturnsFalseWithNullState()
		{
			var transitions = Enumerable.Repeat(StateTransitionHelpers.Create(false), 5);

			var state = new State(null, null, transitions.ToArray(), null);

			var actual = state.TryGetTransition(out var s);

			Assert.False(actual);
			Assert.Null(s);
		}
	}
}
