using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceOnPlayerInput : MonoBehaviour
{
    Rigidbody rb;
    public string key;
    public Vector3 force;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key)) {
            rb.velocity = Vector3.zero;
            rb.AddForce(force, ForceMode.Impulse);
        }
        if (transform.position.y >= 5.5) {
            transform.position += new Vector3(0, -9.5f, 0);
        }
        // gameObject.transform.Rotate(new Vector3(0, 0, rb.velocity.y * 2));
        // if (gameObject.transform.rotation.z <= 45){
        //     gameObject.transform.rotation = new Vector3(0, 0, 45);
        // } 
        // else if (gameObject.transform.rotation.z >= -45) {
            
        // }
    }
}
