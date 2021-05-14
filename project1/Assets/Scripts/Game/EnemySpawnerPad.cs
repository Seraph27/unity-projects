using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;



public class EnemySpawnerPad : MonoBehaviour {
    public GameObject enemyToSpawn;

    public void Start(){
        Debug.Assert(enemyToSpawn != null, "remember to add ebemee to spawn");
        Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
        //write
    }

}