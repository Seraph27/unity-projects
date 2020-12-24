using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public PlayerHp playerHealth;

    private void Start()
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
}