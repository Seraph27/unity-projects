using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObjectOnTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter(Collider c){
        GameObject.Destroy(c.gameObject);
    }
}
