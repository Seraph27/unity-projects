using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletReload : MonoBehaviour
{
    public GameObject toSpawn;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 3; i++){
            Instantiate(toSpawn, transform.position + new Vector3(2*i, 0, -1), Quaternion.Euler(0, 0, 180));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
