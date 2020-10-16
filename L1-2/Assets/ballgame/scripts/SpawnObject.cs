using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject toSpawn;
    float timer = 0.0f;
    public float interval = 0.0f;
    public float range = 0.0f;

    void Update() {
        timer += Time.deltaTime;
        if (timer >= interval) {
            Vector3 offset = new Vector3(
                transform.position.x + UnityEngine.Random.Range(-range, range),
                transform.position.y ,
                transform.position.z + UnityEngine.Random.Range(-range, range));
            Instantiate(toSpawn, offset, Quaternion.identity);
            timer = 0;
        }
    }
}
