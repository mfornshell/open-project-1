using NUnit.Framework;

namespace UOP1.Tests
{
    public class ConditionTests
    {
		[TestCase(true)]
		[TestCase(false)]
		public void Condition_GetStatement_ReturnsCorrectValue(bool value)
		{
			var condition = new MockCondition() { Evaluation = value };

			//returns a cached result, otherwise evaluates the result, caches it, and return
			var actual = condition.Get();

			Assert.AreEqual(value, actual);
		}
    }
}
