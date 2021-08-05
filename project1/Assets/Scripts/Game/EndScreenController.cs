using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreenController : MonoBehaviour
{

    public GameObject textObjectPrefab;
    void Start(){
        textObjectPrefab = GameController.Instance.getPrefabByName("TextCanvas");
        Debug.Log(textObjectPrefab);

        StartCoroutine(RunStats());
    }

    IEnumerator RunStats(){
        yield return new WaitForSeconds(1.5f);
        var textObject = Instantiate(textObjectPrefab, transform.position, Quaternion.identity);
        var tmp = textObject.GetComponentInChildren<TextMeshProUGUI>();
        tmp.SetText("Enemy Killed: " + GameController.totalEnemyKills + "\n\n\n"
        + "-------------------------\n"
        + "Coins Earned: " + GameController.totalEnemyKills * 10
        );

        Debug.Log(tmp);

        
    }
}
