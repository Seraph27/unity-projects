using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayParticleOnKeyPress : MonoBehaviour
{
    public string key;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<ParticleSystem>().Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key)) {
           gameObject.GetComponent<ParticleSystem>().Play();
        }   
        else if (Input.GetKeyUp(key)){
        gameObject.GetComponent<ParticleSystem>().Stop();
        }
    }
}
