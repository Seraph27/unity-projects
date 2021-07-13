using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System;

public class BossTrigger : MonoBehaviour
{

    List<(Vector3, TileBase)> notPassable;
    GameObject player;
    Vector3 triggerPos;
    GameObject bossPrefab;
    GameObject dragonBoss;
    bool hasSpawnedBoss = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameController.Instance.player;
        bossPrefab = GameController.Instance.getPrefabByName("dragonBoss");
        triggerPos = getBossTriggerPosition();
        //Debug.Log(triggerPos);
    }

    void Update() {
        if(!hasSpawnedBoss && (player.transform.position - triggerPos).magnitude < 2 && Input.GetKeyDown(KeyCode.Space)){
            spawnBoss();
        }
    }

    Vector3 getBossTriggerPosition() {
        Tilemap tilemap = GetComponent<Tilemap>();
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
        //Debug.Log("x: " + bounds.size.x + " y: " + bounds.size.y);
        for (int x = bounds.min.x; x < bounds.max.x; x++) {
            for (int y = bounds.min.y; y < bounds.max.y; y++) {                                                                               
                TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));
                if (tile != null && tile.name == "tileset1_9") {
                    Debug.Log(tile.name);
                    Vector3 worldPos = tilemap.GetCellCenterWorld(new Vector3Int(x, y, 0));
                    return worldPos;
                }
            }
        }
        
        throw new KeyNotFoundException();                    
    } 

    void spawnBoss(){
        
        hasSpawnedBoss = true;
        dragonBoss = Instantiate(bossPrefab, triggerPos + new Vector3(0, 5, 0), Quaternion.identity);
        dragonBoss.name = "dragonBoss";

    }
}
