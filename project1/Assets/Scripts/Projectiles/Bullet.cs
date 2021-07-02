using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float power;

    private void OnTriggerEnter2D(Collider2D other) {
        if(GameController.Instance.isWithNotPassableTile(other)){
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
       if(GameController.Instance.isWithNotPassableTile(other.collider)){
            Destroy(gameObject);
        }
    }
}
