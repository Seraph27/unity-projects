using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{   
    public GameObject cylinder;
    public GameObject gunImpact;
    GameObject mainCam;
    bool isFiring;
    //GameObject gunPoint;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = transform.parent.gameObject;
        //gunPoint = transform.Find("gunPoint").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(mainCam.transform.position, mainCam.transform.forward * 100f, Color.green, 0.5f);
        
        if(Input.GetButtonDown("Fire1")){
            isFiring = true;
        }
        // if(Input.GetButtonUp("Fire1")){
        //     isFiring = false;
        // }
    }

    void FixedUpdate(){
        RaycastHit hit;
        if(isFiring && Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, 100f)){
            var c = Instantiate(gunImpact, hit.point, mainCam.transform.rotation);
            var hitGameObjectRB = hit.collider.gameObject.GetComponent<Rigidbody>();
            if(hitGameObjectRB != null){
                hitGameObjectRB.AddForce(mainCam.transform.forward, ForceMode.Impulse);
            }
            //GameObject.Destroy(c, 0.5f);
            isFiring = false;
            // c.transform.localEulerAngles = hit.normal;
            // Debug.Log(hit.normal);
        }
    }
}
