using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SavePositionTile : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D c) {
        if(GameController.Instance.isWithPlayer(c)){
            Debug.Log("working");
            Tilemap tilemap = GameObject.Find("NotPassable").GetComponent<Tilemap>();
            GameController.Instance.deleteTilesWithName(tilemap, "tileset1_66");
            GameController.Instance.savePlayerPositionOnTransition(c.gameObject.transform.position);
        }
    }


}
