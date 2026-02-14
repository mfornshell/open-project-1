using UOP1.StateMachine;

namespace UOP1.Tests
{
	public class MockCondition : Condition
	{
		internal bool Evaluation = false;

		internal MockCondition() { }

		public MockCondition(bool evaluation) => Evaluation = evaluation;

		protected override bool Evaluate() => Evaluation;
	}
}
