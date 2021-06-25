using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    GameObject whitePixel;
    public float maxValue, value;
    GameObject objectToFollow;
    public GameObject damageTextPrefab;
    public Renderer objectToFollowRen;
    public Gradient gradient;

    public void Initalize(GameObject objectToFollow, float hp)
    {
        whitePixel = transform.Find("whitePixel").gameObject;
        this.objectToFollow = objectToFollow;
        maxValue = hp;
        value = hp;
        objectToFollowRen = objectToFollow.GetComponent<Renderer>();
    }

    public void Initalize(GameObject objectToFollow, float hp, float max)
    {
        whitePixel = transform.Find("whitePixel").gameObject;
        this.objectToFollow = objectToFollow;
        maxValue = max;
        value = hp;
        objectToFollowRen = objectToFollow.GetComponent<Renderer>();
        var colorAfterDamage = ColorFromGradient(value / maxValue);
        whitePixel.GetComponent<SpriteRenderer>().color = colorAfterDamage;
    }

    public bool IsAlive(){
        return value > 0;  //true if its alive
    }

    public void ApplyDamage(float damage)
    {
        value -= damage;
        var colorAfterDamage = ColorFromGradient(value / maxValue);
        whitePixel.GetComponent<SpriteRenderer>().color = colorAfterDamage;
        var damageText = Instantiate(damageTextPrefab, transform.position, Quaternion.identity);
        damageText.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(damage.ToString());
        GameObject.Destroy(damageText, 1);
    }

    Color ColorFromGradient (float value){
        return gradient.Evaluate(value);
    }

    public void IncreaseHp(int hp){
        maxValue += hp;
        value += hp;
    }

    void Update(){
        var pos = objectToFollow.transform.position; //+ new Vector3(0, -(objectToFollowRen.bounds.max.y - objectToFollowRen.bounds.min.y)/2 - 0.5f, 0);
        transform.position = pos;

        var ratio = value / maxValue;
        whitePixel.transform.localScale = new Vector3(100 * ratio, 20, 1);
    }

}