using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneName;

    void OnTriggerEnter2D(Collider2D collider) {
        print(collider.gameObject.name);
        print(collider.gameObject.tag);
        if(collider.gameObject.tag == "Player"){
            SceneManager.LoadScene(sceneName);  
        }
        
    }
}
