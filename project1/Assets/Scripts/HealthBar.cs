using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    GameObject objectToFollow;
    public void Initalize(GameObject objectToFollow, int hp)
    {
        healthBar = GetComponent<Slider>();
        this.objectToFollow = objectToFollow;
        healthBar.maxValue = hp;
        healthBar.value = hp;
    }

    public bool IsAlive(){
        return healthBar.value > 0;  //true if its alive
    }

    public void ApplyDamage(float damage)
    {
        healthBar.value -= damage;
        // print("after famaged" + healthBar.value);
    }

    void Update(){
        var pos = objectToFollow.transform.position + new Vector3(0,-1,0);
        healthBar.transform.position = Camera.main.WorldToScreenPoint(pos);
    }

}