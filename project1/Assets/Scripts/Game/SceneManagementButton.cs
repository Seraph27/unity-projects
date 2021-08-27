using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManagementButton : MonoBehaviour
{
    public string sceneName;
    public bool LoadFirstLevel;
    public Button button;

    void onMouseDown() {
        if(LoadFirstLevel){
            SceneManager.LoadScene(GameController.Instance.levelNames[0]);
        }  else{
            SceneManager.LoadScene(sceneName);
        } 
    }

    void OnEnable()
    {
        //Register Slider Events
        button.onClick.AddListener(delegate { onMouseDown(); });
    }

    void OnDisable()
    {
        //Un-Register Slider Events
        button.onClick.RemoveAllListeners();
    }
}
