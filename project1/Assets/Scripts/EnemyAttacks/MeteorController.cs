using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorController : MonoBehaviour
{

    GameObject player;
    ParticleSystem explosion;

    // Start is called before the first frame update
    void Start()
    {
        player = GameController.Instance.player;
        explosion = transform.Find("MeteorExplosion").GetComponent<ParticleSystem>();
    }

    void OnCollisionEnter2D(Collision2D other) {
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        if(GameController.Instance.isWithPlayer(other.collider)){
            player.GetComponent<PlayerController>().hpBarScript.ApplyDamage(50);
        }
        explosion.Play();
        GameObject.Destroy(gameObject, 2);
    }
}
