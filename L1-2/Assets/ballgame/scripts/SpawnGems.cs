using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGems : MonoBehaviour
{
    public GameObject toSpawn;
    float timer = 0.0f;
    float wait = 0.0f;
    bool finishedWaiting = false;
    public float interval = 0.0f;
    public float range = 0.0f;
    public Vector3 axis;


    void Update() {
        if (!finishedWaiting){
            wait += Time.deltaTime;
        }

        if (!finishedWaiting && wait > 1){
            timer += Time.deltaTime;
            if (timer >= interval) {
                Vector3 bottomWallPos = transform.position + new Vector3(
                    UnityEngine.Random.Range(-range, range) * axis.x,
                    UnityEngine.Random.Range(-range, range) * axis.y,
                    UnityEngine.Random.Range(-range, range) * axis.z
                );
                Instantiate(toSpawn, bottomWallPos, Quaternion.identity);
                timer = 0;
            }
        }

    }
}
