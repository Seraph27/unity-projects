using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    GameObject player;
    GameObject explosionAnimationPrefab;
    public GameObject owner;
    // Start is called before the first frame update
    void Start()
    {
        player = GameController.Instance.player;
        explosionAnimationPrefab = GameController.Instance.getPrefabByName("explosionAnimation");
    }

    // Update is called once per frame
    void Update()
    {

        var direction = (Vector2)(player.transform.position - transform.position);
        direction.Normalize();
        transform.position += (Vector3)(direction * Time.deltaTime * 5);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg - 90);   
    }

    void OnTriggerEnter2D(Collider2D c){
        if(c.gameObject == owner){
            return;
        }

        if(GameController.Instance.isWithPlayer(c)){
            player.GetComponent<PlayerController>().hpBarScript.ApplyDamage(10);
        }
        var explosion = Instantiate(explosionAnimationPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, 2);
        Destroy(gameObject);


    }

}
