using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class EnemyController : MonoBehaviour
{
    protected Rigidbody2D rb;
    public GameObject hpBarPrefab;
    protected GameObject hpBar;
    protected HealthBar hpBarScript;
    
    abstract public int GetBaseHp();

    void Start()
    {
        print("is calling");
        rb = gameObject.GetComponent<Rigidbody2D>();
        hpBar = Instantiate(hpBarPrefab);
        hpBarScript = hpBar.transform.Find("healthBar").GetComponent<HealthBar>();
        hpBarScript.Initalize(gameObject, GetBaseHp());  
    }

    virtual protected void Update()
    {
        if(hpBarScript.healthBar.value <= 0){
            Destroy(gameObject);
            Destroy(hpBar);
        }
    }

    void OnTriggerEnter2D(Collider2D c){
        if (c.gameObject.tag == "PlayerProjectile") {
            hpBarScript.DamagePlayer(25); //change
            Destroy(c.gameObject);
            print(hpBarScript.healthBar.value);
        }
    }

}
