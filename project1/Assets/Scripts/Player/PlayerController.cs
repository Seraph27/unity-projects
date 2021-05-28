using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public enum WeaponKind{
    PiuPiuLaser,
    Shotgun,
    Flamethrower,
}

public class Weapon {
    public WeaponKind kind;
    public int damage;
    public Sprite icon;
    public Func<bool, IEnumerator> makeBulletFunc;

    public Weapon(WeaponKind kind, int damage, string iconName, Func<bool, IEnumerator> makeBulletFunc) {
        this.kind = kind;
        this.damage = damage;
        this.icon = GameController.Instance.spriteHolder.getSpriteByName(iconName);
        this.makeBulletFunc = makeBulletFunc;
    }
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
    Sprite[] weaponSheet;
    SpriteRenderer ren;
    public Rigidbody2D rb;
    public GameObject bulletPrefab;
    public float bulletVelocity;
    public HealthBar hpBarScript;
    public float damageMultiplier = 1.0f; 
    public int cash;
    public GameObject cashTextPrefab;
    public GameObject weaponIconPrefab;
    bool isShootingActiveA = false;
    bool isShootingActiveB = false;
    Vector3 playerVelocity;
    GameObject weaponIconA;
    GameObject weaponIconB;
    List<Weapon> weapons = new List<Weapon>();
    int activeWeaponIndexA = 2;
    int activeWeaponIndexB = 1;
    // Start is called before the first frame update
    void Start()
    {
        cash = 100;
        var hpBar = Instantiate(hpBarPrefab);
        hpBarScript = hpBar.GetComponent<HealthBar>();
        hpBarScript.Initalize(gameObject, 100);
        GameController.Instance.spriteHolder.loadSpritesByName("playerSprites");
        GameController.Instance.spriteHolder.loadSpritesByName("flame");
        GameController.Instance.spriteHolder.loadSpritesByName("bullet");
        GameController.Instance.spriteHolder.loadSpritesByName("weapons");
        front = GameController.Instance.spriteHolder.getSpriteByName("frontView");
        side = GameController.Instance.spriteHolder.getSpriteByName("sideView");
        back = GameController.Instance.spriteHolder.getSpriteByName("backView"); 
        ren = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        Instantiate(cashTextPrefab, transform.position, Quaternion.identity);
        weapons.Add(new Weapon(WeaponKind.PiuPiuLaser, 25, "weapons_0", MakePiuPiuBullet));
        weapons.Add(new Weapon(WeaponKind.Shotgun, 10, "weapons_9", MakeShotgunBlast));
        weapons.Add(new Weapon(WeaponKind.Flamethrower, 2, "weapons_18", MakeFlamethrowerFlame));

        weaponIconA = Instantiate(weaponIconPrefab, transform.position, Quaternion.identity);
        weaponIconB = Instantiate(weaponIconPrefab, transform.position, Quaternion.identity);
        weaponIconA.GetComponentInChildren<Image>().sprite = weapons[activeWeaponIndexA].icon;
        weaponIconB.GetComponentInChildren<Image>().sprite = weapons[activeWeaponIndexB].icon;
    }

    void FixedUpdate(){
        rb.velocity = playerVelocity * speed * Time.fixedDeltaTime;
    }
    // Update is called once per frame
    void Update()
    {
        playerVelocity = new Vector3(0,0,0);

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

        playerVelocity.Normalize();


        //if press "e" slotA = false
        if (Input.GetMouseButton(0) && isShootingActiveA == false) {
            //if slotA ac
            isShootingActiveA = true;
            StartCoroutine(weapons[activeWeaponIndexA].makeBulletFunc(true)); 
        } 

        if(!hpBarScript.IsAlive()){
            SceneManager.LoadScene("StartScene");
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
    
    IEnumerator MakePiuPiuBullet(bool isA)
    {
        GameObject bullet;
        Rigidbody2D rb;

        (bullet, rb) = CreateGenericBullet(25 * damageMultiplier, 1, "bullet");
        yield return new WaitForSeconds(0.33f);
        if(isA){
            isShootingActiveA = false;
        } else{
            isShootingActiveB = false;
        }
    }

    IEnumerator MakeShotgunBlast(bool isA) {
        GameObject bullet;
        Rigidbody2D rb;

        for (int i = 0; i < 10; i++) {
            (bullet, rb) = CreateGenericBullet(10 * damageMultiplier, 1, "bullet", 3, UnityEngine.Random.Range(-30, 30));
        }
        yield return new WaitForSeconds(0.75f);
        if(isA){
            isShootingActiveA = false;
        } else{
            isShootingActiveB = false;
        }
    }

    IEnumerator MakeFlamethrowerFlame(bool isA){
        GameObject bullet;
        Rigidbody2D rb;

        (bullet, rb) = CreateGenericBullet(2 * damageMultiplier, 1, "flame", 0.75f, UnityEngine.Random.Range(-5, 5), 0.5f);

        yield return new WaitForSeconds(0.1f);
        if(isA){
            isShootingActiveA = false;
        } else{
            isShootingActiveB = false;
        }
    }



    private (GameObject, Rigidbody2D) CreateGenericBullet(
        float damage,
        float size, 
        string spriteName, 
        float speedMultiplier = 1, 
        float rotationOffset = 0,
        float bulletLife = 0)
    {
        GameObject bullet = new GameObject(spriteName);
        bullet.AddComponent<PlayerBullet>().power = damage;
        var ren = bullet.AddComponent<SpriteRenderer>();
        Rigidbody2D rb = bullet.AddComponent<Rigidbody2D>();
        var circleCollider = bullet.AddComponent<CircleCollider2D>();
        bullet.tag = "PlayerProjectile";
        ren.sprite = GameController.Instance.spriteHolder.getSpriteByName(spriteName); 
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
        if(bulletLife > 0){
            GameObject.Destroy(bullet, bulletLife);
        }
        return (bullet, rb);
    }

   
}
