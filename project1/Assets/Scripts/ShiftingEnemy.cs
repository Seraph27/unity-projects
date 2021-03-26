using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftingEnemy : EnemyController
{


    override public int GetBaseHp(){
        return 200;
    }
    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D c){ 

        if (c.gameObject.tag == "PlayerProjectile") {
            float triggerChance = Random.Range(0, 100);
            print(triggerChance);
            if(triggerChance > 50){
                Vector3 randomVec3 = new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 4f), 0);
                transform.position += randomVec3;
            }
            else{
                GameObject player = GameObject.Find("Player(Clone)");
                if (player != null){
                    PlayerController playerController = player.GetComponent<PlayerController>();
                    hpBarScript.ApplyDamage(25 * playerController.damageMultiplier);
                } 
                Destroy(c.gameObject);
            }
        }
    }
}