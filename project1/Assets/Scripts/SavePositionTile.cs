using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePositionTile : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collider) {
        // print(collider.gameObject.name);
        // print(collider.gameObject.tag);
        if(collider.gameObject.tag == "Player"){
            GameController.Instance.savePlayerPositionOnTransition(collider.gameObject.transform.position);
        }
        
    }
}
