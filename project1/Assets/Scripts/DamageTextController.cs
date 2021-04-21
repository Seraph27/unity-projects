using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextController : MonoBehaviour
{
    private TextMeshPro textMesh;

    private void Awake(){
        textMesh =  transform.GetChild(0).GetComponent<TextMeshPro>();
    }

    public static DamageTextController CreateDamageText(Vector3 position, float damage){
        print("jo" + GameAssets.i.DamageText);
        Transform damageTextTransform = Instantiate(GameAssets.i.DamageText, position, Quaternion.identity);
        DamageTextController damageTextScript = damageTextTransform.GetComponent<DamageTextController>();
        damageTextScript.setText(damage);
        GameObject.Destroy(damageTextTransform.gameObject, 1);
        return damageTextScript;
    }

    public void setText(float damage){
        textMesh.SetText(damage.ToString());
    }
}
