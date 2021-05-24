using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;



public class EnemySpawnerPad : MonoBehaviour {
    public GameObject enemyToSpawn;
    public GameObject enemySpawned;
    int spawnCount;
    bool isCoroutineRunning = false;

    public void Update(){
        if(enemySpawned != null){
            if(!isCoroutineRunning && !enemySpawned.GetComponent<EnemyController>().hpBarScript.IsAlive() && spawnCount < 3){ //not sure if good
                StartCoroutine(RespawnTimerCoroutine());
            }
        }

    }

    IEnumerator RespawnTimerCoroutine(){
        isCoroutineRunning = true;
        print("started" + spawnCount);
        yield return new WaitForSeconds(3);
        enemySpawned = Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
        spawnCount++;
        isCoroutineRunning = false;

    }

}