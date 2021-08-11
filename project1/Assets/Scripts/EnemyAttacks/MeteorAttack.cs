using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class MeteorAttack : MonoBehaviour
{
    public GameObject astroidPrefab;
    GameObject player;
    float reloadDelay;

    void Start()
    {
        astroidPrefab = GameController.Instance.getPrefabByName("Meteor");
        player = GameController.Instance.player;
        StartCoroutine(attackReload());
    }

    IEnumerator attackReload(){
        while(true){
            yield return new WaitForSeconds(1.5f);
            var randomVec3 = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
            var astroid = Instantiate(astroidPrefab, player.transform.position + randomVec3 + new Vector3(0, 5, 0), Quaternion.identity);
            astroid.GetComponent<Rigidbody2D>().AddForce(Vector2.down);
            GameObject.Destroy(astroid, 1.5f);
        }     
    }
}
