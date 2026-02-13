using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UOP1.Tests
{
    public class CharacterStateMachineTestManager : MonoBehaviour
    {
		[SerializeField] InputReader _inputReader;
		[SerializeField] Protagonist _characterPrefab;
		[SerializeField] VoidEventChannelSO _sceneReadyChannel;

		public InputReader InputReader => _inputReader;
		public Protagonist Character { get; private set; }

        void Start()
        {
			//Character = Instantiate(_characterPrefab);
			_inputReader.EnableGameplayInput();
			_sceneReadyChannel?.RaiseEvent();
        }
    }
}
