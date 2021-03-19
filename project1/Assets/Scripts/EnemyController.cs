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
        rb = gameObject.GetComponent<Rigidbody2D>();
        hpBar = Instantiate(hpBarPrefab);
        hpBarScript = hpBar.GetComponent<HealthBar>();
        hpBarScript.Initalize(gameObject, GetBaseHp());  
    }

    virtual protected void Update()
    {
        if(hpBarScript.value <= 0){
            Destroy(gameObject);
            Destroy(hpBar);
        }
    }

    void OnTriggerEnter2D(Collider2D c){
        if (c.gameObject.tag == "PlayerProjectile") {
            GameObject player = GameObject.Find("Player(Clone)");
            if (player != null){
                PlayerController playerController = player.GetComponent<PlayerController>();
                hpBarScript.ApplyDamage(25 * playerController.damageMultiplier);
            } 
            Destroy(c.gameObject);
        }
    }

}
