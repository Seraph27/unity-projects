﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SignController : MonoBehaviour
{

    public GameObject signPrefab;
    public GameObject player;
    public GameObject sign;
    // Start is called before the first frame update
    void Start()
    {
        signPrefab = GameController.Instance.getPrefabByName("Sign");
        sign = Instantiate(signPrefab, transform.position, Quaternion.identity);
        var textMesh = sign.GetComponentInChildren<TextMeshPro>();
        textMesh.SetText("You might have to defeat some enemies for the key to the next level to show up"); 
        sign.SetActive(false);
        player = GameController.Instance.player;

    }

    
    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other) {   
        if(GameController.Instance.isWithPlayer(other)){
            sign.SetActive(true);    
        }  
    }

    private void OnTriggerExit2D(Collider2D other) {   
        if(GameController.Instance.isWithPlayer(other)){
            sign.SetActive(false);    
        }       
    }
}
