using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBar : MonoBehaviour
{

    public float maxValue, value;
    public GameObject damageTextPrefab;
    float originalLocalScaleX;
    float originalLocalScaleY;
    public GameObject dragonBoss;
    public TextMeshPro bossHealthText;
    public SpriteRenderer dragonBossRen;

    public void Initalize(float hp) //for everything else
    {   
        damageTextPrefab = GameController.Instance.getPrefabByName("DamageTextParent");
        maxValue = hp;
        value = hp;
        originalLocalScaleX = transform.localScale.x;
        originalLocalScaleY = transform.localScale.y;
        dragonBoss = GameObject.Find("dragonBoss");
        dragonBossRen = dragonBoss.GetComponentInChildren<SpriteRenderer>();
        bossHealthText = transform.parent.GetChild(1).gameObject.GetComponent<TextMeshPro>();
    }

    public void ApplyDamage(float damage, bool isCritBullet)
    {
        value -= damage;
        StartCoroutine(changeColorOnDmgCoroutine());
        
        var damageText = Instantiate(damageTextPrefab, dragonBoss.transform.position + new Vector3(0, -16, 0), Quaternion.identity);

        var damageTextTMP = damageText.GetComponentInChildren<TextMeshPro>();
        damageTextTMP.color = Color.yellow;
        damageTextTMP.fontSize = 250;
        damageTextTMP.SetText(damage.ToString());

        if(isCritBullet){
            Color critColor;
            if (ColorUtility.TryParseHtmlString("#ff903b", out critColor))
            damageTextTMP.color = critColor;
            damageTextTMP.fontSize = 300;
        }
        GameObject.Destroy(damageText, 1);
    }

    void Update(){

        var ratio = value / maxValue;
        transform.localScale = new Vector3(originalLocalScaleX * ratio, originalLocalScaleY, 1);
        bossHealthText.text = value + " / " + maxValue;
    }

    IEnumerator changeColorOnDmgCoroutine(){
        dragonBossRen.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        dragonBossRen.color = Color.white;
    }
}
