using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveProblemCube : MonoBehaviour
{

    public bool wobble;

    float speed = 4;
    float scaleIncrease = .5f;

    private Vector3 _mainCamPos;
    private Vector3 destination;
    
    Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        destination = GameObject.Find("Destination").transform.position;
        dir = (destination - transform.position).normalized;
        transform.position = Vector3.LerpUnclamped(
            transform.position,
            Camera.main.transform.position,
            Random.Range(-.25f, .25f)
        );
        float offset = 2;
        transform.Translate(
            ((Random.value * 2) - 1) * offset,
            ((Random.value * 2) - 1) * offset, 
            0
        );
        speed = speed * Random.Range(.5f, 1f);
        scaleIncrease *= (Random.value - .25f) * .01f;
        transform.localScale = Vector3.one * Random.Range(.5f, 2f);
       
        _mainCamPos = Camera.main.transform.position;

    }

    // // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space)) {
            transform.position += dir * speed  * Time.deltaTime;
            transform.localScale += Vector3.one * scaleIncrease;
            
            if(wobble) {
                float sin = (Mathf.Sin(Time.time) + 1) / 2;
                float dist = .25f;
                Camera.main.transform.position = _mainCamPos + new Vector3(
                    Mathf.Lerp(-dist, dist, sin),0,0
                );
            }
        }
    }
}
