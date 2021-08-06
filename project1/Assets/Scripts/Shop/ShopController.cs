using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class ShopItem{
    public Func<bool> onSuccessfulPurchaseHandler;
    public Sprite icon; 
    public int cost;
    public float costScale;
    public int currentItemLevel;
    public int maxItemLevel;

    public ShopItem(Func<bool> onSuccessfulPurchaseHandler, Sprite icon, int cost, float costScale, int currentItemLevel, int maxItemLevel)
    {
        this.onSuccessfulPurchaseHandler = onSuccessfulPurchaseHandler;
        this.icon = icon;
        this.cost = (int)(cost * Math.Pow(costScale, currentItemLevel));
        this.costScale = costScale;
        this.currentItemLevel = currentItemLevel;
        this.maxItemLevel = maxItemLevel;
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
        var shopPlayerSpeedIcon = GameController.Instance.spriteHolder.getSpriteByName("weapons_33");
        //add hp
        List<ShopItem> shopItems = new List<ShopItem>();
        shopItems.Add(new ShopItem(
            () => {
                GameController.Instance.globalAttributes.globalPlayerMaxHealth += 20;
                GameController.Instance.globalAttributes.globalPlayerMaxHealthLevel++;
                return true;
            },
            shopPlayerHealthIcon,
            30,
            1.15f,
            GameController.Instance.globalAttributes.globalPlayerMaxHealthLevel,
            10
        ));

        shopItems.Add(new ShopItem(
            () => {
                GameController.Instance.globalAttributes.globalPlayerBaseDamage += 0.1f;
                GameController.Instance.globalAttributes.globalPlayerBaseDamageLevel++;
                return true;
            },
            shopPlayerDamageIcon,
            100,
            1.15f,
            GameController.Instance.globalAttributes.globalPlayerBaseDamageLevel,
            10
        ));

        shopItems.Add(new ShopItem(
            () => {
                GameController.Instance.globalAttributes.globalPlayerSpeed += 20; 
                GameController.Instance.globalAttributes.globalPlayerSpeedLevel++;
                return true;
            },

            shopPlayerSpeedIcon,
            100,
            1.15f,
            GameController.Instance.globalAttributes.globalPlayerSpeedLevel,
            10
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
