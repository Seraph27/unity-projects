using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;



public class EnemySpawnerPad : MonoBehaviour {
    public GameObject enemyToSpawn;
    bool isCoroutineRunning = false;

    public void Update(){
        
        if(enemyToSpawn != null && !isCoroutineRunning){
            StartCoroutine(RespawnTimerCoroutine());
        }

    }

    IEnumerator RespawnTimerCoroutine(){
        isCoroutineRunning = true;
        for(int i = 0; i < 3; i++){
            var enemySpawned = Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
            var e = enemySpawned.GetComponent<EnemyController>();

            while(e.hpBarScript == null){ // init method in enemy controller so we can make sure that hpbar is ready
                yield return new WaitForSeconds(1/60);
            }

            while(e.hpBarScript.IsAlive()){
                yield return new WaitForSeconds(1/60);
            }
            yield return new WaitForSeconds(3);
        }
    }

}