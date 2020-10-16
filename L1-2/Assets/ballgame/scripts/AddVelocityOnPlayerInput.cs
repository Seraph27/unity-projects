using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddVelocityOnPlayerInput : MonoBehaviour
{
    public Vector3 force;
    public string positiveKey;
    public string negativeKey;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(positiveKey)) {
            GetComponent<Rigidbody>().velocity += force;
        }
        else if(Input.GetKey(negativeKey)) {
            GetComponent<Rigidbody>().velocity -= force;
        }
    }

}
