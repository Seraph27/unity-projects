using UnityEngine.SceneManagement;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
    //do stuff
        GameController.Instance.setupGame();
    }
}