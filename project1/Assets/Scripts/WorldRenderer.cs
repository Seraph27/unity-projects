using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRenderer : MonoBehaviour
{
    float spacing = 6.25f;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 16; i++){
            for(int j = 0; j < 16; j++){
                var tile = new GameObject();
                tile.transform.position = new Vector3(i / spacing, j / spacing, 0);
                var ren = tile.AddComponent<SpriteRenderer>();
                ren.color = i%2 == 0 ? Color.red : Color.white;
                ren.sprite = Resources.Load<Sprite>("whitePixel");
                ren.transform.localScale = new Vector3(16, 16, 0);
            }
        }
        
        
        // Camera.main.orthographicSize = 1.5f; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
