using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoringEnemy : EnemyController
{
    public float speed;
    int direction = 1;
    public GameObject powerupPrefab;

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
            Instantiate(powerupPrefab, transform.position, Quaternion.identity);
        }

      base.Update();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        direction *= -1;
    }


}
