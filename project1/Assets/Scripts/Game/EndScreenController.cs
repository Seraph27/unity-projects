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

        var totalCoinsEarned = GameController.totalEnemyKills * 10;
        tmp.SetText("Enemy Killed: " + GameController.totalEnemyKills + " x10\n\n\n"
        + "-------------------------\n"
        + "Coins Earned: " + totalCoinsEarned);


        GameController.Instance.globalAttributes.globalPlayerCurrency += totalCoinsEarned;
        GameController.Instance.saveGlobalsToFile();

        
    }
}
