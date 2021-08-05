using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenController : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other) {
        if(GameController.Instance.isWithPlayer(other)){
            Debug.Log("a");
            //cut to another scene with ease
        }
    }
}
