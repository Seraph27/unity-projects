using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteWhenOffScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void OnBecameInvisible() {
        GameObject.Destroy(gameObject);
        print("hi");
    }

}
