using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnCollision : MonoBehaviour
{
    public string sceneName;
    public string otherTag;

    void OnCollisionEnter(Collision c){
        if (c.gameObject.tag == otherTag) {
            // SceneManager.LoadScene(sceneName);
        }
    }
}
