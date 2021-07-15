using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrailController : MonoBehaviour
{
    GameObject player;

    void Start()
    {
        player = GameController.Instance.player;
        GameObject.Destroy(gameObject, 2);
    }

    void OnTriggerEnter2D(Collider2D c){
        if(GameController.Instance.isWithEnemy(c)){
            return;
        }

        if(GameController.Instance.isWithPlayer(c)){
            player.GetComponent<PlayerController>().hpBarScript.ApplyDamage(20);
        }
        Destroy(gameObject);
    }
}
