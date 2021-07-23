using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class ShopItem{
    public Func<bool> onSuccessfulPurchaseHandler;
    public Sprite icon; 
    public int cost;

    public ShopItem(Func<bool> onSuccessfulPurchaseHandler, Sprite icon, int cost)
    {
        this.onSuccessfulPurchaseHandler = onSuccessfulPurchaseHandler;
        this.icon = icon;
        this.cost = cost;
    }

}

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
        List<ShopItem> shopItems = new List<ShopItem>();
        shopItems.Add(new ShopItem(
            () => {GameController.Instance.globalAttributes.globalPlayerMaxHealth += 20; return true;},
            shopPlayerHealthIcon,
            30 
        ));

        shopItems.Add(new ShopItem(
            () => {GameController.Instance.globalAttributes.globalPlayerBaseDamage += 0.1f; return true;},
            shopPlayerDamageIcon,
            100 
        ));

        Vector3 offset = Vector3.zero;
        foreach (var shopitem in shopItems) {
            var go = Instantiate(shopObjectPrefab, transform.position + offset, Quaternion.identity);
            var shopItemScript = go.GetComponent<OnShopItemClicked>();
            shopItemScript.shopItem = shopitem;
            shopItemScript.updateIconAndText();
            offset += new Vector3(2.5f, 0, 0);
        }
    }
    
    // private GameObject createShopObject<T>(Vector3 spawnPosition) where T : Component{   //spawn position is relative to shopController's position
    //     var go = Instantiate(shopObjectPrefab, transform.position + spawnPosition, Quaternion.identity);
    //     go.AddComponent<T>(); 
    //     return go;
    // }

}
