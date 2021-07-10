using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreasePlayerDamageOnClick : MonoBehaviour
{
    public int itemCost = 100;
    public ShopController shopControllerScript;

    private void Start() {
        shopControllerScript = GameObject.FindObjectOfType<ShopController>();
    }
    // Start is called before the first frame update
    private void OnMouseDown() {
        if(GameController.Instance.globalPlayerCurrency >= itemCost){
            GameController.Instance.globalPlayerBaseDamage += 0.1f;
            GameController.Instance.globalPlayerCurrency -= itemCost;
            itemCost = (int)((float)itemCost * 1.15f);
            shopControllerScript.setUpText(gameObject, itemCost);
            GameController.Instance.saveGlobalsToFile();
        }
        
    }
}
