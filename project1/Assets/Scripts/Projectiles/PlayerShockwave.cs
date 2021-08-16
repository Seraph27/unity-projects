using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShockwave : MonoBehaviour
{

    CircleCollider2D circleCollider2D;
    // Start is called before the first frame update
    void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D c){

        if(!GameController.Instance.isWithPlayer(c)){
            var enemyController = c.GetComponent<EnemyController>();
            if(enemyController != null){
                enemyController.hpBarScript.ApplyDamage(20);
            }
        }
    }

    public void increaseRad(float rad){
        circleCollider2D.radius += rad;
    }
       
    
}
