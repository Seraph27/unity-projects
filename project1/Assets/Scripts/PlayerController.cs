using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum WeaponKind{
    PiuPiuLaser,
    Shotgun,
}

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
    public Rigidbody2D rb;
    public GameObject bulletPrefab;
    public float bulletVelocity;
    public HealthBar hpBarScript;
    public float damageMultiplier = 1.0f; 
    public int cash;
    public GameObject cashTextPrefab;
    WeaponKind activeWeapon = WeaponKind.Shotgun;
    // Start is called before the first frame update
    void Start()
    {
        cash = 100;
        var hpBar = Instantiate(hpBarPrefab);
        hpBarScript = hpBar.GetComponent<HealthBar>();
        hpBarScript.Initalize(gameObject, 100);
        front = Resources.Load<Sprite>("frontView");
        side = Resources.Load<Sprite>("sideView");
        back = Resources.Load<Sprite>("backView");
        ren = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        Instantiate(cashTextPrefab, transform.position, Quaternion.identity);
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

        rb.velocity = playerVelocity.normalized * distanceThisFrame;

        if (Input.GetMouseButtonDown(0)) {
            if(activeWeapon == WeaponKind.PiuPiuLaser) MakePiuPiuBullet();
            else MakeShotgunBlast();
                
        }

        if(!hpBarScript.IsAlive()){
            SceneManager.LoadScene("StartScene");
        }

        //weapon switch
        if (Input.GetKey("1")){ 
            activeWeapon = activeWeapon == WeaponKind.PiuPiuLaser ? WeaponKind.Shotgun : WeaponKind.PiuPiuLaser;
        }
    }

    void OnTriggerEnter2D(Collider2D c) { 
        //bullets
        if (c.gameObject.tag == "EnemyProjectile") {
            hpBarScript.ApplyDamage(0); 
            Destroy(c.gameObject);
        }

        //drops
        if (c.gameObject.tag == "Powerup_Damage") {
            damageMultiplier = 2.0f;
            StartCoroutine(ResetDamageMultiplierCoroutine());
            Destroy(c.gameObject);
        }
    }


    IEnumerator ResetDamageMultiplierCoroutine() {
        yield return new WaitForSeconds(5);
        damageMultiplier = 1.0f;
    }
    
    void MakePiuPiuBullet()
    {
        GameObject bullet;
        Rigidbody2D rb;

        (bullet, rb) = CreateGenericBullet(1, "PlayerBullet");
    }


    void MakeShotgunBlast() {
        GameObject bullet;
        Rigidbody2D rb;

        for (int i = 0; i < 10; i++) {
            (bullet, rb) = CreateGenericBullet(1, "PlayerBullet", 3, UnityEngine.Random.Range(-30, 30));
        }
    }



    private (GameObject, Rigidbody2D) CreateGenericBullet(
        float size, 
        string spriteName, 
        float speedMultiplier = 1, 
        float rotationOffset = 0)
    {
        GameObject bullet = new GameObject(spriteName);
        var ren = bullet.AddComponent<SpriteRenderer>();
        Rigidbody2D rb = bullet.AddComponent<Rigidbody2D>();
        var circleCollider = bullet.AddComponent<CircleCollider2D>();
        bullet.tag = "PlayerProjectile";
        ren.sprite = Resources.Load<Sprite>(spriteName);
        if (damageMultiplier >= 2.0f)
        {
            ren.color = Color.red;
        }
        ren.sortingLayerName = "Projectiles";
        ren.sortingOrder = 0;
        rb.gravityScale = 0;
        bullet.AddComponent<DeleteWhenOffScreen>();
        circleCollider.isTrigger = true;
        circleCollider.radius = size / 10;
        bullet.transform.localScale = new Vector3(size * 3, size * 3, 0);

        var worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);  //bullet shooting
        var direction = (Vector2)(worldMousePos - transform.position);
        direction.Normalize();
        var rotationDegrees = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + rotationOffset;
        var rotationVector =  (Vector2)(Quaternion.Euler(0, 0, rotationDegrees) * Vector2.right);
        bullet.transform.rotation = Quaternion.Euler(0, 0, rotationDegrees);
        bullet.transform.position = transform.position + (Vector3)(rotationVector * 1.0f);
        rb.velocity = rotationVector * 10 * speedMultiplier;
        return (bullet, rb);
    }

   
}
