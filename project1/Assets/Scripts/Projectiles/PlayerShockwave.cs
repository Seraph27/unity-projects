using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShockwave : MonoBehaviour
{

    CircleCollider2D circleCollider2D;
    Grenade grenadeScript;
    // Start is called before the first frame update
    void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        grenadeScript = transform.parent.GetComponent<Grenade>();
        
    }

    void OnTriggerEnter2D(Collider2D c){

        if(!GameController.Instance.isWithPlayer(c)){
            var enemyController = c.GetComponent<EnemyController>();
            if(enemyController != null){
                enemyController.hpBarScript.ApplyDamage(grenadeScript.power, grenadeScript.isCritBullet);
            }
        }
    }

    public void increaseRad(float rad){
        circleCollider2D.radius += rad;
    }
       
    
}
