using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.TestTools;

namespace UOP1.Tests
{
    public class CharacterStateMachineTests
    {
		static readonly string sceneKey = "Assets/Tests/PlayMode/Assets/TestScene.unity";

		InputTestFixture _input = new InputTestFixture();

		[UnitySetUp]
		public IEnumerator Setup()
		{
			_input.Setup();

			var handle = Addressables.LoadSceneAsync(sceneKey);

			if (!handle.IsDone)
				yield return handle;
		}

        [Test]
        public void CharacterStateMachineTestsSimplePasses()
        {
            // Use the Assert class to test conditions
        }

		[UnityTest]
		public IEnumerator AttackPressed_StateMachine_TransitionsToAttack()
		{
			yield return null;
			var manager = Object.FindAnyObjectByType<CharacterStateMachineTestManager>();
			var character = manager.Character;
			var input = manager.InputReader;
		}
    }
}
