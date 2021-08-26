using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OnScreenHealthBarController : MonoBehaviour
{   
    PlayerController playerController;
    TextMeshProUGUI textMeshProUGUI;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameController.Instance.player.GetComponent<PlayerController>();
        textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();

    }

    // Update is called once per frame
    void Update()
    {
        textMeshProUGUI.SetText("HP: " + playerController.hpBarScript.value); 
    }
}
