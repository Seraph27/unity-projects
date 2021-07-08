using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;


public class KeyController : MonoBehaviour
{
    public int killRequiredToUnlock;
    PlayerController playerController; 
    Renderer ren;
    Collider2D c;
    Tilemap tilemap;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameController.Instance.player.GetComponent<PlayerController>();
        tilemap = GameObject.Find("NotPassable").GetComponent<Tilemap>();
        ren = gameObject.GetComponent<Renderer>();
        c = gameObject.GetComponent<Collider2D>();
        ren.enabled = false;
        c.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.enemyKills > killRequiredToUnlock){
           ren.enabled = true;
           c.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(GameController.Instance.isWithPlayer(other)){
            GameController.Instance.deleteTilesWithName(tilemap, "tileset1_66");
            Destroy(gameObject);
        }
    }
}
