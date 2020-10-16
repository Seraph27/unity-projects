using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnTrigger : MonoBehaviour
{
    public string sceneName;
    void OnTriggerEnter(Collider c){
        SceneManager.LoadScene(sceneName);
    }
}


