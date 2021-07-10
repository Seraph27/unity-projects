using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopController : MonoBehaviour
{
    public GameObject cashTextPrefab;
    GameObject shopIconObjectPrefab;
    
    private void Start() {
        Instantiate(cashTextPrefab, transform.position, Quaternion.identity);

        shopIconObjectPrefab = GameController.Instance.getPrefabByName("ShopIcon");
        var shopPlayerHealthObject = Instantiate(shopIconObjectPrefab, transform.position, Quaternion.identity);
        var shopHealthEffectScript = shopPlayerHealthObject.AddComponent<IncreasePlayerHealthOnClick>();
        var textMesh = shopPlayerHealthObject.transform.GetComponentInChildren<TextMeshPro>();
        textMesh.SetText("Cost: " + shopHealthEffectScript.itemCost);
        if(GameController.Instance.globalPlayerCurrency < shopHealthEffectScript.itemCost){
            textMesh.color = Color.red;
        } else{
            textMesh.color = Color.green;
        }


        
    }
    
}
