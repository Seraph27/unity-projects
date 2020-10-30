using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToGameObject : MonoBehaviour
{
    public GameObject goToObject;
    // Update is called once per frame
    void Update()
    {
        transform.position = goToObject.transform.position;
    }
}
