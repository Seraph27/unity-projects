using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorController : MonoBehaviour
{

    GameObject player;
    GameObject explosionAnimationPrefab;

    // Start is called before the first frame update
    void Start()
    {
        player = GameController.Instance.player;
        explosionAnimationPrefab = GameController.Instance.getPrefabByName("explosionAnimation");
    }

    void OnCollisionEnter2D(Collision2D other) {
        GameObject.Destroy(gameObject);
        if(GameController.Instance.isWithPlayer(other.collider)){
            player.GetComponent<PlayerController>().hpBarScript.ApplyDamage(100);
        }
        var explosion = Instantiate(explosionAnimationPrefab, transform.position, Quaternion.identity);
        explosion.transform.localScale += new Vector3(2, 2, 0);
        Destroy(explosion, 2);
    }
}
