using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseHpOnClick : MonoBehaviour
{
    PlayerController playerController;

    void Start(){
        
    }
    // Start is called before the first frame update
    void OnMouseDown()
        {
            var playerController = GameObject.Find("Player(Clone)").GetComponent<PlayerController>();

            if(playerController.cash > 20){
                print("increase hp :)");
                //incrase hp
                playerController.cash -= 20;
            }
            
        }
}
