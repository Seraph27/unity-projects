using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public string upKey;
    public string rightKey;
    public string leftKey;
    public string downKey;
    public float speed;
    Sprite front;
    Sprite side;
    Sprite back;
    SpriteRenderer ren;
    // Start is called before the first frame update
    void Start()
    {
        front = Resources.Load<Sprite>("frontView");
        side = Resources.Load<Sprite>("sideView");
        back = Resources.Load<Sprite>("backView");
        ren = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        var distanceThisFrame = speed * Time.deltaTime;

        if (Input.GetKey(upKey)){ 
            ren.sprite = back;
            transform.position += new Vector3(0,distanceThisFrame,0);
        }
        if (Input.GetKey(rightKey)){
            ren.sprite = side;
             ren.flipX = false;
            transform.position += new Vector3(distanceThisFrame,0,0);
        }
        if (Input.GetKey(leftKey)){
            ren.sprite = side;
            ren.flipX = true;
            transform.position += new Vector3(-distanceThisFrame,0,0);
        }
        if (Input.GetKey(downKey)){
            ren.sprite = front;
            transform.position += new Vector3(0,-distanceThisFrame,0);
        }
    }
}
