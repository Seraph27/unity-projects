using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBulletOnKeyPress : MonoBehaviour
{
    public GameObject toSpawn;
    public string key;
    public int bulletCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key) && bulletCount > 0){
            Instantiate(toSpawn, transform.position + new Vector3(1,0,0), Quaternion.identity);
            bulletCount--;
        }
    }
}
