using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public PlayerHp playerHealth;
    public GameObject player;

    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHp>();
        healthBar = GetComponent<Slider>();
        healthBar.maxValue = playerHealth.maxHp;
        healthBar.value = playerHealth.maxHp;
    }

    public void SetHealth(int hp)
    {
        healthBar.value = hp;
    }

    void Update() {
        var playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(0,-1,0);
        healthBar.transform.position = Camera.main.WorldToScreenPoint(playerPosition);
    }
}