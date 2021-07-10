using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CashController : MonoBehaviour
{
    TextMeshProUGUI cashText;
    void Start () {
        cashText = GetComponent<TextMeshProUGUI>();
    }
    // Update is called once per frame
    void Update()
    {
        cashText.text = "" + GameController.Instance.globalPlayerCurrency;
    }
}
