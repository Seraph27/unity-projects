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
        Debug.Log(GameController.Instance.globalAttributes.globalPlayerCurrency);
        if(GameController.Instance.globalAttributes.globalPlayerCurrency >= shopItem.cost && shopItem.currentItemLevel < shopItem.maxItemLevel){
            shopItem.onSuccessfulPurchaseHandler();
            GameController.Instance.globalAttributes.globalPlayerCurrency -= shopItem.cost;
            shopItem.cost = Mathf.RoundToInt(shopItem.cost * shopItem.costScale);
            shopItem.currentItemLevel++;
            GameController.Instance.saveGlobalsToFile();
        }
        updateIconAndText();
        
    }

    public void updateIconAndText(){
        var textMesh = transform.GetComponentInChildren<TextMeshPro>();
        textMesh.SetText("Cost: " + shopItem.cost + "\nLv. " + shopItem.currentItemLevel + " / " + shopItem.maxItemLevel); 

        if(GameController.Instance.globalAttributes.globalPlayerCurrency < shopItem.cost || shopItem.currentItemLevel >= shopItem.maxItemLevel){
            textMesh.color = Color.red;
        } else{
            textMesh.color = Color.green;
        }

        GetComponent<SpriteRenderer>().sprite = shopItem.icon;
    }
}
