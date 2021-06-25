using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    GameObject player;
    public GameObject explosionAnimationPrefab;
    public GameObject owner;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {

        var direction = (Vector2)(player.transform.position - transform.position);
        direction.Normalize();
        transform.position += (Vector3)(direction * 0.02f);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg - 90);   
    }

    void OnTriggerEnter2D(Collider2D c){
        if(c.gameObject == owner){
            return;
        }

        if(GameController.Instance.isWithPlayer(c)){
            player.GetComponent<PlayerController>().hpBarScript.ApplyDamage(10);
        }
        Instantiate(explosionAnimationPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);


    }

}
