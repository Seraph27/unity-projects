using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    int direction = 1;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
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
       
    }

    void OnCollisionEnter2D(Collision2D collision) {
        direction *= -1;
    }

    void OnTriggerEnter2D(Collider2D collider){
        GameObject.Destroy(gameObject);
    }
}
