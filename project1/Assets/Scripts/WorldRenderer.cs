using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WorldRenderer : MonoBehaviour
{
    float spacing = 5f;
    // Start is called before the first frame update
    void Start()
    {
        var tileSprites = Resources.LoadAll<Sprite>("tileset1");
        var LevelStorer = new GameObject("LevelStorer");
        for(int i = 0; i < 16; i++){
            for(int j = 0; j < 16; j++){
                var tile = new GameObject("Tile " + i.ToString() + " " + j.ToString());  //start from bottom left
                var randomNumber = UnityEngine.Random.value;
                tile.transform.parent = LevelStorer.transform;
                tile.transform.localPosition = new Vector3(i / spacing, j / spacing, 0);
                var ren = tile.AddComponent<SpriteRenderer>();
                if(i == 0 || i == 15 || j == 0 || j == 15){
                    ren.sprite = tileSprites[147]; 
                    tile.AddComponent<BoxCollider2D>(); 
                }
                else if(randomNumber > 0.9f){
                    ren.sprite = tileSprites[3];  
                    tile.AddComponent<BoxCollider2D>();
                }
                else{
                    ren.sprite = tileSprites[2];       
                }
                
                // ren.transform.localScale = new Vector3(1, 1, 0);
            }
        }
        LevelStorer.transform.localScale = new Vector3(5,5,0);
        
        // Camera.main.orthographicSize = 1.5f; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
