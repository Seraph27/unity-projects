using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Phase{
    Spike,
    Minions,
    Attack,
}

public class BossController : MonoBehaviour
{
    public GameObject hpBarPrefab;
    protected GameObject hpBar;
    public HealthBar hpBarScript;
    public Phase currentPhase = Phase.Spike;
    public GameObject player;
    public GameObject spikePrefab;
    // Start is called before the first frame update
    void Start()
    {
        hpBar = Instantiate(hpBarPrefab);
        hpBarScript = hpBar.GetComponent<HealthBar>();
        hpBarScript.Initalize(gameObject, 300); 
        player = GameController.Instance.player;
        StartCoroutine(PhaseCoroutine()); 
        
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log(currentPhase);
    }

    void OnTriggerEnter2D(Collider2D c){  //player dealt dmg
        if (GameController.Instance.isWithPlayerBullet(c)) {
            hpBarScript.ApplyDamage(c.gameObject.GetComponent<PlayerBullet>().power);
            Destroy(c.gameObject);
        }
    }

    IEnumerator PhaseCoroutine(){
        var spikeCoroutine = StartCoroutine(MakeSpikeCoroutine());
        while(hpBarScript.value > 200){
            yield return new WaitForSeconds(1/60);
        }
        StopCoroutine(spikeCoroutine);

        currentPhase = Phase.Minions;
        var minionCoroutine = StartCoroutine(MinionCoroutine());
        while(hpBarScript.value > 100){
            yield return new WaitForSeconds(1/60);
        }
        StopCoroutine(minionCoroutine);

        currentPhase = Phase.Attack;
        var attackCoroutine = StartCoroutine(AttackCoroutine());
        while(hpBarScript.value > 0){
            yield return new WaitForSeconds(1/60);
        }
        StopCoroutine(attackCoroutine);

        Destroy(hpBar);
        Destroy(gameObject);
    }

    IEnumerator MakeSpikeCoroutine(){
        while(true){
            Instantiate(spikePrefab, player.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(3);
        }

    }

    IEnumerator MinionCoroutine(){
        yield return new WaitForSeconds(3);
    }

    IEnumerator AttackCoroutine(){
        yield return new WaitForSeconds(3);
    }
}
