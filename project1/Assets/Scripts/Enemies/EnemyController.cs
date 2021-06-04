using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class EnemyController : MonoBehaviour
{
    protected Rigidbody2D rb;
    public GameObject hpBarPrefab;
    protected GameObject hpBar;
    public HealthBar hpBarScript;
    protected SpriteRenderer ren;


    abstract public int GetBaseHp();

    virtual protected void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        hpBar = Instantiate(hpBarPrefab);
        hpBarScript = hpBar.GetComponent<HealthBar>();
        hpBarScript.Initalize(gameObject, GetBaseHp());  
        ren = GetComponent<SpriteRenderer>();
        
    }

    virtual protected void Update()
    {

        if(hpBarScript.value <= 0){
            Destroy(gameObject);
            Destroy(hpBar);
        }
    }

    void OnTriggerEnter2D(Collider2D c){  //player dealt dmg
        if (c.gameObject.tag == "PlayerProjectile") {
            hpBarScript.ApplyDamage(c.gameObject.GetComponent<PlayerBullet>().power);
            Destroy(c.gameObject);
        }
    }

}





/*

ideas:
just one scene and teleport pads transports player to another area? where they don't see cause everytime i make something on scene 1 i need to copy to scene 2
(DONE)


enemy ideas:

shifing enemy:  (DONE)
have a certain chance to shift itself in its radius if the enemy detects a projectile is going to hit it   

Meteor enemy:

drops a meteor and it shows a red circle of where its gonna land, so player needs to leave the area or they will take dmg

Trail enemy: (DONE)

leaves a trail of poison anywhere it goes and does dmg when player steps on it

bouce enemy:
bounce arounds the screen

dashing enemy:

follows the player and dashes through the player when its close enough doing damage (Done)

earthquake enemy:

hits the ground and send spikes of rocks out of all directions dealing damage

sHIELD enemy:

generates a shield around it that takes 5 hits to destroy and reflects the bullet back at the player

shockwave enemy:

sends out a shockwave that pushes the player away

gravity enemy:

sucks the player in and explodes on it



Boss:

larger hp bar (or a health bar that shows up at the top of the screen with exact health point and stuff)
single room 
use multiple skills from previous enemy




*/