using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    GameObject healthBar;
    public float maxValue, value;
    GameObject objectToFollow;
    public void Initalize(GameObject objectToFollow, int hp)
    {
        healthBar = transform.Find("whitePixel").gameObject;
        this.objectToFollow = objectToFollow;
        maxValue = hp;
        value = hp;
    }

    public bool IsAlive(){
        return value > 0;  //true if its alive
    }

    public void ApplyDamage(float damage)
    {
        value -= damage;
        // print("after famaged" + healthBar.value);
    }

    public void IncreaseHp(int hp){
        maxValue += hp;
        value += hp;
    }

    void Update(){
        var pos = objectToFollow.transform.position + new Vector3(0,-1,0);
        healthBar.transform.position = pos;

        var ratio = value / maxValue;
        healthBar.transform.localScale = new Vector3(100 * ratio, 20, 1);
    }

}