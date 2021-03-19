using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public GameObject shopOverlayPrefab;
    GameObject shopOverlay;

    void Start(){
        shopOverlay = Instantiate(shopOverlayPrefab, Vector3.zero, Quaternion.identity, Camera.main.transform);
        shopOverlay.SetActive(false);
    }
    

    void OnTriggerEnter2D(Collider2D c){
        print("triggered");
        if(c.gameObject.tag == "Player"){
            print(shopOverlay);
            print(c.gameObject.GetComponent<PlayerController>().cash);
            shopOverlay.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D c){
        if(c.gameObject.tag == "Player"){
            shopOverlay.SetActive(false);
        }
    }
}
