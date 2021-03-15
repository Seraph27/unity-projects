using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnEntites : MonoBehaviour {
    public GameObject playerPrefab;
    public GameObject boringEnemyPrefab;
    public GameObject interestingEnemyPrefab;

    void Start () {
        Tilemap tilemap = GetComponent<Tilemap>();

        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
        Debug.Log("x: " + bounds.size.x + " y: " + bounds.size.y);
        for (int x = bounds.min.x; x < bounds.max.x; x++) {
            for (int y = bounds.min.y; y < bounds.max.y; y++) {                                                                               
                TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));
                if (tile != null) {
                    Vector3 worldPos = tilemap.CellToWorld(new Vector3Int(x, y, 0));

                    if(tile.name == "tileset1_53"){
                        Instantiate(playerPrefab, worldPos + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                    }  
                    if(tile.name == "tileset1_52"){   
                        Instantiate(boringEnemyPrefab, worldPos + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                    } 
                    if(tile.name == "tileset1_114"){
                        Instantiate(interestingEnemyPrefab, worldPos + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                    }                                    
                } 
            }
        }        
    }   
}
