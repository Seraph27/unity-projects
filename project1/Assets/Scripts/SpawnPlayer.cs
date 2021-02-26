using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnPlayer : MonoBehaviour {
    public GameObject playerPrefab;
    void Start () {
        Tilemap tilemap = GetComponent<Tilemap>();

        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++) {
            for (int y = 0; y < bounds.size.y; y++) {                                                                               
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null) {
                    if(tile.name == "tileset1_53"){
                        Vector3 localPos = new Vector3(x - bounds.size.x / 2, y - bounds.size.y / 2 , 0);
                        Instantiate(playerPrefab, localPos , Quaternion.identity);
                        Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                    }                  
                } 
            }
        }        
    }   
}
