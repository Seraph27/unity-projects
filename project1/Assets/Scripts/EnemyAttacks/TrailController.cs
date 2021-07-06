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
        player = GameController.Instance.player;
        owner = GameObject.Find("TrailEnemy");
        GameObject.Destroy(gameObject, seconds);
    }

    void OnTriggerEnter2D(Collider2D c){
        if(c.gameObject == owner){
            return;
        }

        if(GameController.Instance.isWithPlayer(c)){
            player.GetComponent<PlayerController>().hpBarScript.ApplyDamage(50);
        }
        Destroy(gameObject);
    }
}
