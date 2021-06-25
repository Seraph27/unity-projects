using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneName;

    void OnTriggerEnter2D(Collider2D c) {
        // print(collider.gameObject.name);
        // print(collider.gameObject.tag);
        if(GameController.Instance.isWithPlayer(c)){
            GameController.Instance.SavePlayerState();
            SceneManager.LoadScene(sceneName);  
        }
        
    }
}
