using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float power;
    public bool isCritBullet;

    void OnTriggerEnter2D(Collider2D c) {
        if(!GameController.Instance.isWithPlayer(c) && !GameController.Instance.isWithPlayerBullet(c)){
            Debug.Log("working" + c.gameObject.name);
            var explosion = transform.parent.GetComponentInChildren<ParticleSystem>();
            var rb = transform.parent.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            if(explosion != null){
                explosion.Play();
            }
            Destroy(gameObject);
        }
    }

}
