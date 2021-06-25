using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePositionTile : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D c) {
        if(GameController.Instance.isWithPlayer(c)){
            GameController.Instance.savePlayerPositionOnTransition(c.gameObject.transform.position);
        }
        
    }
}
