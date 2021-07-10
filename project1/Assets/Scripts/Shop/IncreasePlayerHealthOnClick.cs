using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreasePlayerHealthOnClick : MonoBehaviour
{
    public int itemCost = 30;
    // Start is called before the first frame update
    private void OnMouseDown() {
        if(GameController.Instance.globalPlayerCurrency > itemCost){
            GameController.Instance.globalPlayerMaxHealth += 20;
            GameController.Instance.globalPlayerCurrency -= itemCost;
            Debug.Log("INCREASING");
            GameController.Instance.saveGlobalsToFile();
        }
        
    }
}
