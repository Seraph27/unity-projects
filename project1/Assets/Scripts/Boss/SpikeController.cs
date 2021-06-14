using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour
{
    bool isInSpike = false;
    SpriteRenderer ren;
    // Start is called before the first frame update
    void Start()
    {
        ren = GetComponent<SpriteRenderer>();
        StartCoroutine(SpikeCoroutine());
    }
    
    IEnumerator SpikeCoroutine(){
        Debug.Log("RUNNING");
        yield return new WaitForSeconds(1);
        ren.color = Color.red;
        for(int i = 0; i < 10; i++){
            if(isInSpike){
                GameController.Instance.player.GetComponent<PlayerController>().hpBarScript.ApplyDamage(0.1f);
            }
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(gameObject);

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(GameController.Instance.isWithPlayer(other)){
            isInSpike = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(GameController.Instance.isWithPlayer(other)){
            isInSpike = false;
        }
    }
}
