using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWall : MonoBehaviour
{
    public GameObject toSpawn;
    float timer = 0.0f;
    public float interval = 0.0f;
    public float range = 0.0f;
    public Vector3 axis;
    public float wallInterval = 0.0f;

    void Update() {
        // howto get a script on another gameobject
        // GameObject.Find("bird").GetComponent<BulletManager>().bulletCount

        timer += Time.deltaTime;
        if (timer >= interval) {
            Vector3 bottomWallPos = transform.position + new Vector3(
                UnityEngine.Random.Range(-range, range) * axis.x,
                UnityEngine.Random.Range(-range, range) * axis.y,
                UnityEngine.Random.Range(-range, range) * axis.z
            );
            Instantiate(toSpawn, bottomWallPos, Quaternion.identity);

            Vector3 topWallPos = bottomWallPos + new Vector3(0,12+ UnityEngine.Random.Range(0, wallInterval),0);
            if (topWallPos.y - bottomWallPos.y < 12.5){
                topWallPos = bottomWallPos + new Vector3(0,12.5f,0);
            }
            Instantiate(toSpawn, topWallPos, Quaternion.identity);
            timer = 0;
        }
    }
}
