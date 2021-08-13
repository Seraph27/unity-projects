using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum TankEnemyMode{
    Follow,
    Preparing,
    SmashAttack,
}

public class TankEnemy : EnemyController
{
    protected bool isActive = false;
    TankEnemyMode mode = TankEnemyMode.Follow;
    float timer;
    bool hasAttacked = false;
    GameObject shockWavePrefab;
    GameObject shockwave;

    override public int GetBaseHp(){
        return 500;
    }

    override protected void Start()
    {
        shockWavePrefab = GameController.Instance.getPrefabByName("Shockwave");
        base.Start();
    }
    // Update is called once per frame
    override protected void Update()
    {
        var distanceToPlayer = (Vector2)(playerController.transform.position - transform.position);
        timer += Time.deltaTime;

        if (mode == TankEnemyMode.Follow){ 
            transform.position += (Vector3)(distanceToPlayer * Time.deltaTime * 0.75f);
            if(distanceToPlayer.magnitude < 3 && timer > 3){
                timer = 0;
                mode = TankEnemyMode.Preparing;
            }
        } 
        
        if(mode == TankEnemyMode.Preparing){ 
            //do nothing for now
            if(timer > 1){
                timer = 0;
                mode = TankEnemyMode.SmashAttack;
                hasAttacked = false;
            }
        }

        if(mode == TankEnemyMode.SmashAttack){ 
            if(!hasAttacked){
                shockwave = Instantiate(shockWavePrefab, transform.position, Quaternion.identity);
                StartCoroutine(ShockWaveSpreadCoroutine());
                
                hasAttacked = true;
            }

            if(timer > 5){
                timer = 0;
                mode = TankEnemyMode.Follow; 
            }
        }

        base.Update();
    }

    IEnumerator ShockWaveSpreadCoroutine(){
        GameObject.Destroy(shockwave, 2);
        while(true){
            Debug.Log("started3");
            shockwave.transform.localScale += new Vector3(0.1f, 0.1f, 0);
            yield return new WaitForSeconds(0.01f);
        }
        

    }
}