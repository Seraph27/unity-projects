using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject toSpawn;
    float timer = 0.0f;
    public float interval = 0.0f;
    public float range = 0.0f;
    public Vector3 axis;

    void Update() {
        timer += Time.deltaTime;
        if (timer >= interval) {
            Vector3 newpos = transform.position + new Vector3(
                UnityEngine.Random.Range(-range, range) * axis.x,
                UnityEngine.Random.Range(-range, range) * axis.y,
                UnityEngine.Random.Range(-range, range) * axis.z
            );
            Instantiate(toSpawn, newpos, Quaternion.identity);
            timer = 0;
        }
    }
}
