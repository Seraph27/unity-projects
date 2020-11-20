using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPositionOnKeyPress : MonoBehaviour
{
    public string key;
    public int bullet;
    public float x;
    public float y;
    public float z;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key) && bullet > 0) {
            transform.position += new Vector3(x, y, z); 
            bullet--;
        }
    }
}
