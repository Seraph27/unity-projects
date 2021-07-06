using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CashController : MonoBehaviour
{
    TextMeshProUGUI cashText;

    PlayerController playerController;
    void Start () {
        cashText = GetComponent<TextMeshProUGUI>();
        playerController = GameController.Instance.player.GetComponent<PlayerController>();
    }
    // Update is called once per frame
    void Update()
    {
        cashText.text = "" + playerController.cash;
    }
}
