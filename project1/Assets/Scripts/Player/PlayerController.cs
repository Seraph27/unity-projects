using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Linq;

public enum WeaponKind{
    PiuPiuLaser,
    Shotgun,
    Flamethrower,
}

public class Weapon {
    public WeaponKind kind;
    public int damage;
    public Sprite icon;
    public Func<IEnumerator> makeBulletFunc;
    public static Dictionary<WeaponKind, string> weaponIcons = new Dictionary<WeaponKind, string>(){
        {WeaponKind.PiuPiuLaser, "weapons_0"},
        {WeaponKind.Shotgun, "weapons_9"},
        {WeaponKind.Flamethrower, "weapons_18"},
    };
    public static Weapon make_weapon(WeaponKind kind, PlayerController playerController){
        if(kind == WeaponKind.PiuPiuLaser){
            return new Weapon(WeaponKind.PiuPiuLaser, 25, weaponIcons[kind], playerController.MakePiuPiuBullet);
        } else if(kind == WeaponKind.Shotgun){
            return new Weapon(WeaponKind.Shotgun, 10, weaponIcons[kind], playerController.MakeShotgunBlast);
        } else if(kind == WeaponKind.Flamethrower){
            return new Weapon(WeaponKind.Flamethrower, 2, weaponIcons[kind], playerController.MakeFlamethrowerFlame);
        } else{
            throw new NotImplementedException();
        }
    }

    public static GameObject make_drop(Vector3 position, WeaponKind gunType){
        GameObject go = new GameObject();
        go.transform.position = position;
        var ren = go.AddComponent<SpriteRenderer>();
        go.AddComponent<GunDropController>().gunType = gunType;
        ren.sprite = GameController.Instance.spriteHolder.getSpriteByName(weaponIcons[gunType]);
        ren.sortingLayerName = "GUI";
        go.tag = "Weapon";
        go.transform.localScale += new Vector3(5, 5, 0);
        
        return go;
    }

    public Weapon(WeaponKind kind, int damage, string iconName, Func<IEnumerator> makeBulletFunc) {
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
    public GameObject cashTextPrefab;
    public GameObject weaponIconPrefab;
    public GameObject iconFramePrefab;
    bool isShootingActive = false;
    bool isSlotAActive = true;
    Vector3 playerVelocity;
    GameObject weaponIconA;
    GameObject weaponIconB;
    GameObject iconFrame;
    public List<Weapon> weapons = new List<Weapon>();
    int activeWeaponIndexA = 0;
    int activeWeaponIndexB = 1;
    public GameObject weaponDropPrefab;
    GameObject flamethrower;
    public int enemyKills;
    // Start is called before the first frame update
    void Init()
    {
        var hpBar = Instantiate(hpBarPrefab);
        hpBarScript = hpBar.GetComponent<HealthBar>();
        GameController.Instance.spriteHolder.loadSpritesByName("playerSprites");
        GameController.Instance.spriteHolder.loadSpritesByName("flame");
        GameController.Instance.spriteHolder.loadSpritesByName("bullet");
        GameController.Instance.spriteHolder.loadSpritesByName("weapons");
        front = GameController.Instance.spriteHolder.getSpriteByName("frontView");
        side = GameController.Instance.spriteHolder.getSpriteByName("sideView");
        back = GameController.Instance.spriteHolder.getSpriteByName("backView"); 
        ren = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        weaponIconA = Instantiate(weaponIconPrefab, transform.position, Quaternion.identity);
        weaponIconB = Instantiate(weaponIconPrefab, transform.position, Quaternion.identity);
        weaponIconB.GetComponentInChildren<Image>().transform.position += new Vector3(Screen.width * 0.07f, 0, 0);
        iconFrame = Instantiate(iconFramePrefab, transform.position, Quaternion.identity);
    }

    public void RestorePlayerState(List<WeaponKind> savedWeaponKinds, float savedHealth)
    {
        Init();

        if(savedWeaponKinds == null) {
            savedWeaponKinds = new List<WeaponKind>();
            savedWeaponKinds.Add(WeaponKind.PiuPiuLaser);
            savedWeaponKinds.Add(WeaponKind.Shotgun);
        }
        if(savedHealth == 0){
            savedHealth = GameController.Instance.globalPlayerMaxHealth;
        }
        
        foreach(var weaponKind in savedWeaponKinds){
            weapons.Add(Weapon.make_weapon(weaponKind, this));
            
        }
        weaponIconA.GetComponentInChildren<Image>().sprite = this.weapons[activeWeaponIndexA].icon;
        weaponIconB.GetComponentInChildren<Image>().sprite = this.weapons[activeWeaponIndexB].icon;

        hpBarScript.Initalize(gameObject, savedHealth, GameController.Instance.globalPlayerMaxHealth);
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


        if(Input.GetKeyDown("1")){
            var screenWidth = Screen.width;
            if(isSlotAActive){
                iconFrame.GetComponentInChildren<Image>().transform.position += new Vector3(screenWidth * 0.07f, 0, 0);
            } else{
                iconFrame.GetComponentInChildren<Image>().transform.position -= new Vector3(screenWidth * 0.07f, 0, 0);
            }

            isSlotAActive = !isSlotAActive;

        }
        if (Input.GetMouseButton(0) && isShootingActive == false) {
            isShootingActive = true;
            if(isSlotAActive){
                StartCoroutine(weapons[activeWeaponIndexA].makeBulletFunc()); 
            } else{
                StartCoroutine(weapons[activeWeaponIndexB].makeBulletFunc()); 
            }

        } 

        //weapon swapping
        if(Input.GetKeyDown("e")){ 
            var weaponList = GameObject.FindGameObjectsWithTag("Weapon");        //this or use a collider onTrigger to check. 
            GameObject closestWeapon = null;
            float distance = Mathf.Infinity;
            Vector3 position = transform.position;
 
            foreach (GameObject go in weaponList)
            {
                Vector3 diff = go.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    closestWeapon = go;
                    distance = curDistance;
                }
            }
            
            if(closestWeapon != null){
                var closestWeaponType = closestWeapon.GetComponent<GunDropController>().gunType;

                if(isSlotAActive){
                    Weapon.make_drop(transform.position, weapons[activeWeaponIndexA].kind);
                    weapons[0] = Weapon.make_weapon(closestWeaponType, this);
                    weaponIconA.GetComponentInChildren<Image>().sprite = weapons[activeWeaponIndexA].icon;

                } else{
                    Weapon.make_drop(transform.position, weapons[activeWeaponIndexB].kind);
                    weapons[1] = Weapon.make_weapon(closestWeaponType, this);
                    weaponIconB.GetComponentInChildren<Image>().sprite = weapons[activeWeaponIndexB].icon;
                } 

                GameObject.Destroy(closestWeapon);
            }
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
    
    public IEnumerator MakePiuPiuBullet()
    {
        GameObject bullet;
        Rigidbody2D rb;

        (bullet, rb) = CreateGenericBullet(25 * damageMultiplier, 1, "bullet");
        yield return new WaitForSeconds(0.33f);

        isShootingActive = false;
    }

    public IEnumerator MakeShotgunBlast() {
        GameObject bullet;
        Rigidbody2D rb;

        for (int i = 0; i < 10; i++) {
            (bullet, rb) = CreateGenericBullet(10 * damageMultiplier, 1, "bullet", 3, UnityEngine.Random.Range(-30, 30));
        }
        yield return new WaitForSeconds(0.75f);

        isShootingActive = false;
    }

    public IEnumerator MakeFlamethrowerFlame(){
        GameObject bullet;
        Rigidbody2D rb;

        (bullet, rb) = CreateGenericBullet(2 * damageMultiplier, 1, "flame", 0.75f, UnityEngine.Random.Range(-5, 5), 0.5f);

        yield return new WaitForSeconds(0.1f);

        isShootingActive = false;
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
        bullet.AddComponent<Bullet>().power = damage * GameController.Instance.globalPlayerBaseDamage;
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
