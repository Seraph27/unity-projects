using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreasePlayerHealthOnClick : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnMouseDown() {
        GameController.Instance.globalPlayerMaxHealth += 20;
        Debug.Log("INCREASING");
        GameController.Instance.saveGlobalsToFile();
    }
}
