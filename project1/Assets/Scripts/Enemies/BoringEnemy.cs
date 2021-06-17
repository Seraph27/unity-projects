using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoringEnemy : EnemyController
{
    public float speed;
    int direction = 1;

    override public int GetBaseHp(){
        return 200;
    }
    // Update is called once per frame
    override protected void Update()
    {
        var distanceThisFrame = speed * Time.deltaTime;
        var playerVelocity = new Vector3(0,0,0);
        
        if (transform.position.x > 5.5) {
            direction = -1;
        }
        if (transform.position.x < -5.5) {
            direction = 1;
        }

        playerVelocity = new Vector3(direction,0,0);

        rb.velocity = playerVelocity * distanceThisFrame;

        if(hpBarScript.value <= 0){
            if(shouldMakeDrop(0.5f)){
                Weapon.make_drop(transform.position, WeaponKind.Flamethrower);
            }    
        }

      base.Update();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        direction *= -1;
    }


}
