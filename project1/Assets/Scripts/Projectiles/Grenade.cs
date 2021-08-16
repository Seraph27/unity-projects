using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    ParticleSystem meteorExplosion;
    public float power;
    public bool isCritBullet;
    GameObject grenade;
    Coroutine explodeCoroutine;
    GameObject playerShockwavePrefab;
    PlayerShockwave playerShockwave;
    bool hasExplode = false;

    private void Start() {
        
        explodeCoroutine = StartCoroutine(ExplodeCoroutine());
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        StartCoroutine(GrenadeCoroutine(rb));   

        playerShockwave = GetComponentInChildren<PlayerShockwave>();
        
        grenade = transform.Find("bullet").gameObject;
        meteorExplosion = GetComponentInChildren<ParticleSystem>();
        ParticleSystem.MainModule main = meteorExplosion.main;
        main.startLifetimeMultiplier *= 0.5f;

        playerShockwavePrefab = GameController.Instance.getPrefabByName("PlayerShockwave");
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(!GameController.Instance.isWithPlayer(other) && !GameController.Instance.isWithPlayerBullet(other)){
            explode();
            hasExplode = true;
        }
    }

    void explode(){  
        if(!hasExplode){
            meteorExplosion.Play();
            playerShockwave.increaseRad(1.5f);
            StopCoroutine(explodeCoroutine);
            Destroy(grenade);
            Destroy(gameObject, 0.5f);
        }
        
    }

    void OnCollisionEnter2D(Collision2D other) {   
        meteorExplosion.Play();
    }

    IEnumerator GrenadeCoroutine(Rigidbody2D rb){
        while(true){        
            rb.velocity *= new Vector2(0.93f, 0.93f);
            yield return new WaitForSeconds(0.05f);      
        }
    }

    IEnumerator ExplodeCoroutine(){
        yield return new WaitForSeconds(1);
        explode();
        hasExplode = true;
    }
}
