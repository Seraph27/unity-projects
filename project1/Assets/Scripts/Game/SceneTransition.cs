using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    
    void OnTriggerEnter2D(Collider2D c) {   //This triggers when we step on the pad to the next level. 
        if(GameController.Instance.isWithPlayer(c)){
            GameController.Instance.SavePlayerState();
            if(this.tag == "PadToNextLevel"){
                GameController.Instance.addCompletedScenes(SceneManager.GetActiveScene().name);
                GameController.Instance.currentLevelIndex++;
                Debug.Log(GameController.Instance.currentLevelIndex);
                SceneManager.LoadScene(GameController.Instance.levelNames[GameController.Instance.currentLevelIndex]); 
            } else{
                GameController.Instance.currentLevelIndex--;
                Debug.Assert(GameController.Instance.currentLevelIndex >= 0, "negative level index");
                Debug.Log(GameController.Instance.currentLevelIndex);
                SceneManager.LoadScene(GameController.Instance.levelNames[GameController.Instance.currentLevelIndex]); 
            }
            
            
             
        }
        
    }
}
