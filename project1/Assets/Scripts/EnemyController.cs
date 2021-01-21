using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    int direction = 1;
    Rigidbody2D rb;
    public GameObject hpBarPrefab;
    GameObject hpBar;
    HealthBar hpBarScript;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        hpBar = Instantiate(hpBarPrefab);
        hpBarScript = hpBar.transform.Find("healthBar").GetComponent<HealthBar>();
        hpBarScript.DelayedStart();
        hpBarScript.SetMaxHp(200);
        
    }

    // Update is called once per frame
    void Update()
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
       
        hpBarScript.FollowEntity(gameObject.tag);
        if(hpBarScript.healthBar.value <= 0){
            Destroy(gameObject);
            Destroy(hpBar);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        direction *= -1;
    }


    public string otherTag;

    void OnTriggerEnter2D(Collider2D c){
        if (c.gameObject.tag == otherTag) {
            hpBarScript.DamagePlayer(25); //change
            Destroy(c.gameObject);
            print(hpBarScript.healthBar.value);
        }
    }

}
