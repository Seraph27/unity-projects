using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrailController : MonoBehaviour
{
    public float seconds;
    GameObject player;
    public GameObject owner;

    void Start()
    {
        player = GameObject.Find("Player");
        owner = GameObject.Find("TrailEnemy");
        StartCoroutine(SelfDestruct());
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D c){
        if(c.gameObject == owner){
            return;
        }

        if(c.gameObject.tag == "Player"){
            player.GetComponent<PlayerController>().hpBarScript.ApplyDamage(50);
        }
        Destroy(gameObject);
    }
}
