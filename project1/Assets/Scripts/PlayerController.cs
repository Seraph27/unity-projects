using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject hpBarPrefab;
    public string upKey;
    public string rightKey;
    public string leftKey;
    public string downKey;
    public float speed;
    Sprite front;
    Sprite side;
    Sprite back;
    SpriteRenderer ren;
    Rigidbody2D rb;
    public GameObject bulletPrefab;
    public float bulletVelocity;
    // Start is called before the first frame update
    void Start()
    {

        // add a health bar component
        // init hp bar with min and max health
        // store a reference to hp bar
        // hpbar.getHealth()
        // hpbar.changeHealth()
        // hpbar.isAlive()

        var hpBar = Instantiate(hpBarPrefab);
        var hpBarScript = hpBar.transform.Find("healthBar").GetComponent<HealthBar>();
        hpBarScript.playerHealth = gameObject.GetComponent<PlayerHp>();
        hpBarScript.player = gameObject;
        hpBarScript.DelayedStart();
        gameObject.GetComponent<PlayerHp>().healthBar = hpBarScript; 
        front = Resources.Load<Sprite>("frontView");
        side = Resources.Load<Sprite>("sideView");
        back = Resources.Load<Sprite>("backView");
        ren = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        
        print(Camera.main.orthographicSize);
        
    }

    // Update is called once per frame
    void Update()
    {
        var distanceThisFrame = speed * Time.deltaTime;
        var playerVelocity = new Vector3(0,0,0);


        if (Input.GetKey(upKey)){ 
            ren.sprite = back;
            playerVelocity += new Vector3(0,1,0);
        }
        if (Input.GetKey(rightKey)){
            ren.sprite = side;
            ren.flipX = false;
            playerVelocity += new Vector3(1,0,0);
        }
        if (Input.GetKey(leftKey)){
            ren.sprite = side;
            ren.flipX = true;
            playerVelocity += new Vector3(-1,0,0);
        }
        if (Input.GetKey(downKey)){
            ren.sprite = front;
            playerVelocity += new Vector3(0,-1,0);
        }

        rb.velocity = playerVelocity * distanceThisFrame;


        var worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var direction = (Vector2)(worldMousePos - transform.position);
        direction.Normalize();
        if (Input.GetMouseButtonDown(0)) {
            makeBullet();
            // var bullet = Instantiate (bulletPrefab,
            //              transform.position + (Vector3)(direction * 1.0f),
            //              Quaternion.identity);
            // bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletVelocity;
        }

        
    }

    void makeBullet() {
        var bullet = new GameObject("playerBullet");
        var ren = bullet.AddComponent<SpriteRenderer>();
        var rb = bullet.AddComponent<Rigidbody2D>();
        var circleCollider = bullet.AddComponent<CircleCollider2D>();
        ren.sprite = Resources.Load<Sprite>("playerBullet"); 
        rb.gravityScale = 0;
        bullet.AddComponent<DeleteWhenOffScreen>();
        circleCollider.isTrigger = true;
        bullet.transform.localScale = new Vector3(3, 3, 0);
        var worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var direction = (Vector2)(worldMousePos - transform.position);
        direction.Normalize();
        bullet.transform.position = transform.position + (Vector3)(direction * 1.0f);
        bullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg);
        rb.velocity = direction * bulletVelocity;
    }
}
