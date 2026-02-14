using NUnit.Framework;
using UOP1.StateMachine;

namespace UOP1.Tests
{
	public class StateConditionTests
	{
		[TestCase(true)]
		[TestCase(false)]
		public void StateCondition_IsMet_ReturnsTrue(bool value)
		{
			var condition = new MockCondition() { StatementValue = value };

			//expected is important for certain states,
			//ie IsWalking to IsIdle checks for IsMoving == false to transition
			var stateCondition = new StateCondition(null, condition, value);

			var actual = stateCondition.IsMet();
			Assert.True(actual);
		}

		[TestCase(true)]
		[TestCase(false)]
		public void StateCondition_IsNotMet_ReturnsFalse(bool value)
		{
			//just returns the opposite of expectedResult
			var condition = new MockCondition() { StatementValue = !value };

			//StateMachine parameter should eventually not need to be passed down
			//since it is only used for debugging, instead register an event callback if needed
			var stateCondition = new StateCondition(null, condition, value);

			var actual = stateCondition.IsMet();
			Assert.False(actual);
		}
	}
}
