using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OnShopItemClicked : MonoBehaviour
{
    public ShopController shopControllerScript;
    public ShopItem shopItem;

    private void Start() {
        shopControllerScript = GameObject.FindObjectOfType<ShopController>();
    }
    // Start is called before the first frame update
    private void OnMouseDown() {
        if(GameController.Instance.globalAttributes.globalPlayerCurrency >= shopItem.cost){
            shopItem.onSuccessfulPurchaseHandler();
            GameController.Instance.globalAttributes.globalPlayerCurrency -= shopItem.cost;
            // itemCost = (int)((float)itemCost * 1.15f);
            updateIconAndText();
            GameController.Instance.saveGlobalsToFile();
        }
        
    }

    public void updateIconAndText(){
        var textMesh = transform.GetComponentInChildren<TextMeshPro>();
        textMesh.SetText("Cost: " + shopItem.cost);       
        if(GameController.Instance.globalAttributes.globalPlayerCurrency < shopItem.cost){
            textMesh.color = Color.red;
        } else{
            textMesh.color = Color.green;
        }

        GetComponent<SpriteRenderer>().sprite = shopItem.icon;
    }
}
