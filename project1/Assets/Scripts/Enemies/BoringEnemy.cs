using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoringEnemy : EnemyController
{
    int direction = 1;

    override public int GetBaseHp(){
        return 200;
    }
    // Update is called once per frame
    override protected void Update()
    {
        if (transform.position.x >= 5.5) {
            direction = -1;
        }
        if (transform.position.x < -5.5) {
            direction = 1;
        }
        var velocity = new Vector3(direction,0,0);
        
        rb.velocity = velocity * 100 *  Time.deltaTime;

        base.Update();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        direction *= -1;
    }


}
