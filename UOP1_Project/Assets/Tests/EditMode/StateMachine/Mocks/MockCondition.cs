using UOP1.StateMachine;

namespace UOP1.Tests
{
	public class MockCondition : Condition
	{
		internal bool StatementValue = false;

		protected override bool Statement() => StatementValue;
	}
}
