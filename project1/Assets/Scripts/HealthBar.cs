using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;

    public void DelayedStart()
    {
        healthBar = GetComponent<Slider>();
    }

    public void SetMaxHp(int hp){
        healthBar.maxValue = hp;
        healthBar.value = hp;
    }

    public void IsPlayerAlive(){
        if(healthBar.value <= 0) {
            SceneManager.LoadScene("StartScene");
        }
    }

    public void DamagePlayer(int damage)
    {
        healthBar.value -= damage;
        // print("after famaged" + healthBar.value);
    }

    public void FollowEntity(string tag){
        var pos = GameObject.FindGameObjectWithTag(tag).transform.position + new Vector3(0,-1,0);
        healthBar.transform.position = Camera.main.WorldToScreenPoint(pos);
    }

}