using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextController : MonoBehaviour
{
    private TextMeshPro textMesh;
    public GameObject damageText;

    private void Awake(){
        textMesh =  transform.GetChild(0).GetComponent<TextMeshPro>();
    }

    public void CreateDamageText(Vector3 position, float damage){
        var damageTextObject = Instantiate(damageText, position, Quaternion.identity);
        textMesh.SetText(damage.ToString());
        GameObject.Destroy(damageTextObject.gameObject, 1);
    }

}
//this does not work why