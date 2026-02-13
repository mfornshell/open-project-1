using NUnit.Framework;

namespace UOP1.Tests
{
    public class ConditionTests
    {
		[TestCase(true)]
		[TestCase(false)]
		public void Condition_GetStatement_ReturnsCorrectValue(bool value)
		{
			var condition = new MockCondition() { StatementValue = value };

			//returns a _cachedStatement and calculates it with Statement() if _isCached is null;
			var actual = condition.GetStatement();

			Assert.AreEqual(value, actual);
		}
    }
}
