namespace UOP1.StateMachine
{
	public class StateTransition : IStateComponent
	{
		private State _targetState;
		private StateCondition[] _conditions;
		private int[] _resultGroups;
		private bool[] _results;

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
		}

		/// <summary>
		/// Checks wether the conditions to transition to the target state are met.
		/// </summary>
		/// <param name="state">Returns the state to transition to. Null if the conditions aren't met.</param>
		/// <returns>True if the conditions are met.</returns>
		public bool TryGetTransiton(out State state)
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



			int count = _resultGroups.Length;
			for (int i = 0, idx = 0; i < count && idx < _conditions.Length; i++)
			{
				for (int j = 0; j < _resultGroups[i]; j++, idx++)
				{
					if (j == 0)
					{
						//gets initial evaluation
						_results[i] = _conditions[idx].IsMet();
					}
					else
					{
						//keep evaluating for each condition and update
						//if any are false, stays false
						//can probably early return out if this?
						//idx is updated and kept track of in outer loop
						_results[i] = _results[i] && _conditions[idx].IsMet();
					}
				}
			}

			bool ret = false;
			//continues through if ret is still false
			//if results[i] evaluates to true at all, return true
			for (int i = 0; i < count && !ret; i++)
				ret = ret || _results[i];

#if UNITY_EDITOR
			_targetState._stateMachine?._debugger.TransitionEvaluationEnd(ret, _targetState._actions);
#endif

			return ret;
		}

		internal void ClearConditionsCache()
		{
			for (int i = 0; i < _conditions.Length; i++)
				_conditions[i]._condition.ClearCache();
		}
	}
}
