using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// This class is responsible for starting the game by loading the persistent managers scene 
/// and raising the event to load the Main Menu
/// </summary>
public class InitializationLoader : MonoBehaviour
{
	//may need to figure out if these also need to be AssetReferences?
	[SerializeField] private GameSceneSO _persistentManagersScene;
	[SerializeField] private GameSceneSO _mainMenuScene;

	[Header("Broadcasting on")]
	[SerializeField] AssetReference _loadMenu;

	IEnumerator Start()
	{
		yield return _persistentManagersScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);

		var handle = _loadMenu.LoadAssetAsync<LoadEventChannelSO>();
		yield return _loadMenu.LoadAssetAsync<LoadEventChannelSO>().WaitForCompletion();
		LoadMainMenu(handle);
	}

	void LoadMainMenu(AsyncOperationHandle<LoadEventChannelSO> handle)
	{
		handle.Result.RaiseEvent(_mainMenuScene, true);
		SceneManager.UnloadSceneAsync(0); //Initialization is the only scene in BuildSettings, thus it has index 0
	}
}
