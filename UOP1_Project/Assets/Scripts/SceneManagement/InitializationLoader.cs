using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is responsible for starting the game by loading the persistent managers scene 
/// and raising the event to load the Main Menu
/// </summary>
public class InitializationLoader : MonoBehaviour
{
	[SerializeField] private GameSceneSO _persistentManagersScene;
	[SerializeField] private GameSceneSO _mainMenuScene;

	[Header("Broadcasting on")]
	[SerializeField] LoadEventChannelSO _loadMenu;

	IEnumerator Start()
	{
		//Load the persistent managers scene
		yield return _persistentManagersScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
		LoadMainMenu();
		SceneManager.UnloadSceneAsync(0); //Initialization is the only scene in BuildSettings, thus it has index 0
	}

	void LoadMainMenu() => _loadMenu.RaiseEvent(_mainMenuScene, true);
}
