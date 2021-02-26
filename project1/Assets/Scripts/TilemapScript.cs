using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapScript : MonoBehaviour
{
    
    void OnTriggerEnter2D(Collider2D c){
        if(c.gameObject.tag == "PlayerProjectile" || c.gameObject.tag == "EnemyProjectile"){
            Destroy(c.gameObject); 
        }
    }
}
