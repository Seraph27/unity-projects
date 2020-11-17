using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObjectOnCollision : MonoBehaviour
{
    public string otherTag;
    void OnTriggerEnter(Collider c){
        if (c.gameObject.tag == otherTag) {     
            GameObject.Destroy(c.gameObject);
        }
    }
}
