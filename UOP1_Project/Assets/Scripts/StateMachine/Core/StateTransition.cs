namespace UOP1.StateMachine
{
	public class StateTransition : IStateComponent
	{
		class StateConditionGroup
		{
			readonly StateCondition[] _conditions;

			internal StateConditionGroup(StateCondition[] conditions) => _conditions = conditions;

			internal bool IsAllMet()
			{
				for (int i = 0; i < _conditions.Length; i++)
				{
					if (!_conditions[i].IsMet())
						return false;
				}
				return true;
			}
		}

		readonly struct Range
		{
			public readonly int Start, End;

			public Range(int start, int end) => (Start, End) = (start, end);
		}


		private State _targetState;
		private StateCondition[] _conditions;
		
		private int[] _resultGroups;
		private bool[] _results;


		private StateConditionGroup[] _conditionGroups; //implementation 1
		private Range[] _ranges; //implementation 2

		internal StateTransition() { }
		public StateTransition(State targetState, StateCondition[] conditions, int[] resultGroups = null)
		{
			Init(targetState, conditions, resultGroups);
		}

		internal void Init(State targetState, StateCondition[] conditions, int[] resultGroups = null)
		{
			_targetState = targetState;
			_conditions = conditions;
			_resultGroups = resultGroups != null && resultGroups.Length > 0 ? resultGroups : new int[] { conditions.Length };
			_results = new bool[_resultGroups.Length];

			_conditionGroups = CreateConditionGroups();
			_ranges = CreateRanges();
		}

		private StateConditionGroup[] CreateConditionGroups()
		{
			var conditionGroups = new StateConditionGroup[_resultGroups.Length];
			var index = 0;
			for (int i = 0; i < _resultGroups.Length; i++)
			{
				int value = _resultGroups[i];
				var group = new StateCondition[value];
				for (int j = 0; j < value; j++)
				{
					group[j] = _conditions[index];
					index++;
				}
				conditionGroups[i] = new StateConditionGroup(group);
			}
			return conditionGroups;
		}

		Range[] CreateRanges()
		{
			var ranges = new Range[_resultGroups.Length];
			var start = 0;

			for (int i = 0; i < _resultGroups.Length; i++)
			{
				int value = _resultGroups[i];
				var end = start + value - 1;
				ranges[i] = new Range(start, end);
				start = end + 1;
			}

			return ranges;
		}

		/// <summary>
		/// Checks wether the conditions to transition to the target state are met.
		/// </summary>
		/// <param name="state">Returns the state to transition to. Null if the conditions aren't met.</param>
		/// <returns>True if the conditions are met.</returns>
		public bool TryGetTransition(out State state)
		{
			state = ShouldTransition() ? _targetState : null;
			return state != null;
		}

		public void OnStateEnter()
		{
			for (int i = 0; i < _conditions.Length; i++)
				_conditions[i]._condition.OnStateEnter();
		}

		public void OnStateExit()
		{
			for (int i = 0; i < _conditions.Length; i++)
				_conditions[i]._condition.OnStateExit();
		}

		private bool ShouldTransition()
		{
#if UNITY_EDITOR
			_targetState._stateMachine?._debugger.TransitionEvaluationBegin(_targetState._originSO.Name);
#endif

			bool shouldTransition = false;

			shouldTransition = ShouldTransitionNew();

			//for (int i = 0; i < _conditionGroups.Length; i++)
			//{
			//	if (_conditionGroups[i].IsAllMet())
			//	{
			//		shouldTransition = true;
			//		break;
			//	}
			//}

#if UNITY_EDITOR
			_targetState._stateMachine?._debugger.TransitionEvaluationEnd(shouldTransition, _targetState._actions);
#endif

			return shouldTransition;
		}

		bool ShouldTransitionNew()
		{
			for (int i = 0; i < _ranges.Length; i++)
			{
				if (IsAllMet(_ranges[i]))
				{
					return true;
				}
			}
			return false;

			bool IsAllMet(Range range)
			{
				for (int j = range.Start; j <= range.End; j++)
				{
					if (!_conditions[j].IsMet())
						return false;
				}
				return true;
			}
		}

		internal void ClearConditionsCache()
		{
			for (int i = 0; i < _conditions.Length; i++)
				_conditions[i]._condition.ClearCache();
		}
	}
}
