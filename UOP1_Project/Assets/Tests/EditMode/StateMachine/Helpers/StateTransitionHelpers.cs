using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UOP1.StateMachine;

namespace UOP1.Tests
{
	internal static class StateTransitionHelpers
	{
		internal static StateTransition Create(bool returnsTrue = true)
		{
			var condition = new StateCondition(null, new MockCondition(true), returnsTrue);
			return new StateTransition(new State(), new StateCondition[] { condition });
		}
	}
}
