using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Bullet
{
    GameObject meteorExplosionPrefab;
    ParticleSystem meteorExplosion;
    GameObject grenadeParent;

    private void Start() {

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        StartCoroutine(GrenadeCoroutine(rb));

        grenadeParent = GameController.Instance.getPrefabByName("EmptyGameObject");
        var grenadeShell = Instantiate(grenadeParent, transform.position, Quaternion.identity);
        transform.parent = grenadeShell.transform;

        meteorExplosionPrefab = GameController.Instance.getPrefabByName("MeteorExplosion");
        var meteorExplosionObject = Instantiate(meteorExplosionPrefab, transform.position, Quaternion.identity);
        meteorExplosionObject.transform.parent = grenadeShell.transform;
        meteorExplosion = meteorExplosionObject.GetComponent<ParticleSystem>();
        
        
    }

    override protected void OnTriggerEnter2D(Collider2D other) {
        
        base.OnTriggerEnter2D(other);
        meteorExplosion.Play();
        Debug.Log("DWAHOUI");
    }

    override protected void OnCollisionEnter2D(Collision2D other) {
        
        base.OnCollisionEnter2D(other);
        meteorExplosion.Play();
        Debug.Log("DWAHOUI");
    }

    IEnumerator GrenadeCoroutine(Rigidbody2D rb){
        while(true){        
            rb.velocity *= new Vector2(0.9f, 0.9f);
            yield return new WaitForSeconds(0.05f);      
        }
        

    }
}
