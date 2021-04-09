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
            print("Clicked");
            var playerController = GameObject.Find("Player").GetComponent<PlayerController>();

            if(playerController.cash >= 20){
                playerController.hpBarScript.IncreaseHp(50); //needs fix but i guess its ok for now
                playerController.cash -= 20;
            }
            
        }
}
