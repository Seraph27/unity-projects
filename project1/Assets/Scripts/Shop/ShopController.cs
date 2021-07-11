using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopController : MonoBehaviour
{
    public GameObject cashTextPrefab;
    GameObject shopObjectPrefab;
    
    private void Start() {
        Instantiate(cashTextPrefab, transform.position, Quaternion.identity);

        shopObjectPrefab = GameController.Instance.getPrefabByName("ShopIcon");
        GameController.Instance.spriteHolder.loadSpritesByName("weapons");
        var shopPlayerDamageIcon = GameController.Instance.spriteHolder.getSpriteByName("weapons_1");
        var shopPlayerHealthIcon = GameController.Instance.spriteHolder.getSpriteByName("weapons_3");
        //add hp
        var shopPlayerHealthObject = createShopObject<IncreasePlayerHealthOnClick>(new Vector3(0, 0, 0));
        var shopPlayerHealthScript = shopPlayerHealthObject.GetComponent<IncreasePlayerHealthOnClick>();
        setUpText(shopPlayerHealthObject, shopPlayerHealthScript.itemCost); 
        shopPlayerHealthObject.GetComponent<SpriteRenderer>().sprite = shopPlayerHealthIcon;

        //add dmg
        var shopPlayerDamageObject = createShopObject<IncreasePlayerDamageOnClick>(new Vector3(2.5f, 0, 0));
        var shopPlayerDamageScript = shopPlayerDamageObject.GetComponent<IncreasePlayerDamageOnClick>();
        setUpText(shopPlayerDamageObject, shopPlayerDamageScript.itemCost); 
        shopPlayerDamageObject.GetComponent<SpriteRenderer>().sprite = shopPlayerDamageIcon;

        
    }
    
    private GameObject createShopObject<T>(Vector3 spawnPosition) where T : Component{   //spawn position is relative to shopController's position
        var go = Instantiate(shopObjectPrefab, transform.position + spawnPosition, Quaternion.identity);
        go.AddComponent<T>(); 
        return go;
    }

    public void setUpText(GameObject gameObject, int itemCost){
        var textMesh = gameObject.transform.GetComponentInChildren<TextMeshPro>();
        textMesh.SetText("Cost: " + itemCost);       
        if(GameController.Instance.globalPlayerCurrency < itemCost){
            textMesh.color = Color.red;
        } else{
            textMesh.color = Color.green;
        }
        Debug.Log("WORKING");
    }
    
}
