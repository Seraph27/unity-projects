using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SavePositionTile : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D c) {
        if(GameController.Instance.isWithPlayer(c)){
            //Debug.Log("working"); 
            GameController.Instance.savePlayerPositionOnTransition(c.gameObject.transform.position);
        }
    }


}
