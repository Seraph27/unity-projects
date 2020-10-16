using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnKeyPress : MonoBehaviour
{
    public string sceneName;
    public string key;
    void Update(){
        if (Input.GetKeyDown(key)){
            print("Hi");
            SceneManager.LoadScene(sceneName);
        }
    }

}

