using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerHp : MonoBehaviour
{

    public int maxHp;
    public string otherTag;
    int currentHp;
    
    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetKeyDown( KeyCode.Space ) ) // test
        {
            DamagePlayer(10);
        }

        if(currentHp <= 0) {
            SceneManager.LoadScene("StartScene");
        }
    }

    void OnTriggerEnter2D(Collider2D c) {
        Debug.Log("lol");
        if (c.gameObject.tag == otherTag) {
            DamagePlayer(10);
            Destroy(c.gameObject);
        }
    }

    public void DamagePlayer(int damage)
    {
        currentHp -= damage;
        healthBar.SetHealth(currentHp);
    }
}
