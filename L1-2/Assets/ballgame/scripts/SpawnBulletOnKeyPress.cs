using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBulletOnKeyPress : MonoBehaviour
{
    public GameObject toSpawn;
    public string key;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key)){
            Instantiate(toSpawn, transform.position + new Vector3(1,0,0), Quaternion.identity);
        }
    }
}
