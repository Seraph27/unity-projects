using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneName;

    void OnTriggerEnter2D(Collider2D c) {   //This triggers when we step on the pad to the next level. 
        if(GameController.Instance.isWithPlayer(c)){
            if(this.tag == "PadToNextLevel"){
                GameController.Instance.addCompletedScenes(SceneManager.GetActiveScene().name);
            }
            
            GameController.Instance.SavePlayerState();
            SceneManager.LoadScene(sceneName);  
        }
        
    }
}
