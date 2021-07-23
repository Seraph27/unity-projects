using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public string sceneName;
    public bool LoadFirstLevel;
    
    void OnMouseDown() {
        if(LoadFirstLevel){
            SceneManager.LoadScene(GameController.Instance.levelNames[0]);
        }  else{
            SceneManager.LoadScene(sceneName);
        }

        
    }


}
