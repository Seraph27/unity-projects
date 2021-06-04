using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;



public class SpawnEntites : MonoBehaviour {
    public GameObject playerPrefab;
    public GameObject boringEnemyPrefab;
    public GameObject interestingEnemyPrefab;
    public GameObject shiftingEnemyPrefab;

    public List<(Vector3, TileBase)> getTilePositions(){
        var tiles = new List<(Vector3, TileBase)>();
        Tilemap tilemap = GetComponent<Tilemap>();

        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
        Debug.Log("x: " + bounds.size.x + " y: " + bounds.size.y);
        for (int x = bounds.min.x; x < bounds.max.x; x++) {
            for (int y = bounds.min.y; y < bounds.max.y; y++) {                                                                               
                TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));
                if (tile != null) {
                    Vector3 worldPos = tilemap.CellToWorld(new Vector3Int(x, y, 0));
                    tiles.Add((worldPos, tile));
                }
            }
        }
        return tiles;
    }
    
    public GameObject spawnPlayer(){
        GameObject player = null;
        var tiles = getTilePositions();
                
        foreach ((var worldPos, var tile) in tiles) {
            if(tile.name == "tileset1_53"){
                player = Instantiate(playerPrefab, worldPos + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);   //blue orb
                player.name = "Player";
                player.transform.position = worldPos + new Vector3(0.5f, 0.5f, 0);
            } 
        }

        return player;
    }
    public void spawnEnemies() {
        var tiles = getTilePositions();

        foreach ((var worldPos, var tile) in tiles) {
            if(tile.name == "tileset1_52"){  
                GameObject enemySpawnerGO = new GameObject(); 
                enemySpawnerGO.transform.position = worldPos + new Vector3(0.5f, 0.5f, 0);
                var enemySpawnerScript = enemySpawnerGO.AddComponent<EnemySpawnerPad>();
                enemySpawnerScript.enemyToSpawn = boringEnemyPrefab;
            } 
            if(tile.name == "tileset1_114"){
                GameObject enemySpawnerGO = new GameObject(); 
                enemySpawnerGO.transform.position = worldPos + new Vector3(0.5f, 0.5f, 0);
                var enemySpawnerScript = enemySpawnerGO.AddComponent<EnemySpawnerPad>();
                enemySpawnerScript.enemyToSpawn = interestingEnemyPrefab;
            }      
            if(tile.name == "tileset1_116"){
                GameObject enemySpawnerGO = new GameObject(); 
                enemySpawnerGO.transform.position = worldPos + new Vector3(0.5f, 0.5f, 0);
                var enemySpawnerScript = enemySpawnerGO.AddComponent<EnemySpawnerPad>();
                enemySpawnerScript.enemyToSpawn = shiftingEnemyPrefab;
            }  
        }                              
    }   
}
