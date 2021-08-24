using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveController : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameController.Instance.player;
        StartCoroutine(ShockWaveSpreadCoroutine());
        GameObject.Destroy(gameObject, 2);
    }

    void OnTriggerEnter2D(Collider2D c){

        if(GameController.Instance.isWithPlayer(c)){
            player.GetComponent<PlayerController>().hpBarScript.ApplyDamage(30);
        }
    }

    IEnumerator ShockWaveSpreadCoroutine(){
        
        while(true){
            Debug.Log("started3");
            transform.localScale += new Vector3(0.1f, 0.1f, 0);
            yield return new WaitForSeconds(0.01f);
        }
        

    }
}
