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
    Laser,

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
        {WeaponKind.Laser, "weapons_18"},
    };
    public static Weapon make_weapon(WeaponKind kind, PlayerWeaponController playerWeaponController){
        if(kind == WeaponKind.PiuPiuLaser){
            return new Weapon(WeaponKind.PiuPiuLaser, 25, weaponIcons[kind], playerWeaponController.MakePiuPiuBullet);
        }   else if(kind == WeaponKind.Shotgun){
            return new Weapon(WeaponKind.Shotgun, 10, weaponIcons[kind], playerWeaponController.MakeShotgunBlast);
        }   else if(kind == WeaponKind.Flamethrower){
            return new Weapon(WeaponKind.Flamethrower, 2, weaponIcons[kind], playerWeaponController.MakeFlamethrowerFlame);
        }   else if(kind == WeaponKind.Laser){
            return new Weapon(WeaponKind.Laser, 10, weaponIcons[kind], playerWeaponController.MakeLaserBeam);
        }   else{
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

public class PlayerWeaponController : MonoBehaviour
{
    GameObject linePrefab;
    LineRenderer lineRenderer;
    PlayerController playerController;
    float playerCritChance;
    float playerCritMultiplier;
    public float damageMultiplier = 1.0f; 
    bool isShootingActive = false;

    private void Start() {
        linePrefab = GameController.Instance.getPrefabByName("LineLaser");
        lineRenderer = Instantiate(linePrefab, Vector3.zero, Quaternion.identity).GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        playerController = GetComponent<PlayerController>();
        playerCritChance = GameController.Instance.globalAttributes.globalPlayerCritChance;
        playerCritMultiplier = GameController.Instance.globalAttributes.globalPlayerCritMultiplier;
    }
    
    private void Update() {
        if (Input.GetMouseButton(0) && isShootingActive == false) {
            isShootingActive = true;
            if(playerController.isSlotAActive){
                StartCoroutine(playerController.weapons[playerController.activeWeaponIndexA].makeBulletFunc()); 
            } else{
                StartCoroutine(playerController.weapons[playerController.activeWeaponIndexB].makeBulletFunc()); 
            }

        } 
    }
    public IEnumerator MakePiuPiuBullet()
    {
        GameObject bullet;
        Rigidbody2D rb;
        GameController.Instance.playAudio("PistolSoundEffect");

        (bullet, rb) = CreateGenericBullet(25 * damageMultiplier, playerCritChance, playerCritMultiplier, 1, "bullet", 2);
        yield return new WaitForSeconds(0.33f);

        isShootingActive = false;

    }

    public IEnumerator MakeShotgunBlast() {
        GameObject bullet;
        Rigidbody2D rb;
        GameController.Instance.playAudio("ShotgunSoundEffect"); 

        for (int i = 0; i < 10; i++) {
            (bullet, rb) = CreateGenericBullet(10 * damageMultiplier, playerCritChance, playerCritMultiplier, 1, "bullet", 0.5f, 2, UnityEngine.Random.Range(-30, 30));
        }
        yield return new WaitForSeconds(0.75f);

        isShootingActive = false;
    }

    public IEnumerator MakeLaserBeam(){

        //GameController.Instance.playAudio("ShotgunSoundEffect"); 

        var worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);  //bullet shooting
        var direction = (Vector3)(worldMousePos - transform.position) * 10;
        Debug.Log(direction);
        Debug.DrawRay(transform.position, direction, Color.red, 1f);
        Debug.Log("shoot");

        lineRenderer.enabled = true;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, 110f);
        
        var gradient = new Gradient();
        Color startColor = Color.red;
        Color endColor = Color.yellow;
        float alpha = 1.0f;
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(startColor, 0.0f), new GradientColorKey(endColor, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lineRenderer.colorGradient = gradient; 

        if(hits.Length > 1){ //(use linerenderer to show cast)
            var target = hits[1];
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, target.point);
            
            if(GameController.Instance.isWithEnemy(target.collider)){
                var targetHealthScript = target.collider.gameObject.GetComponent<EnemyController>().hpBarScript;
                targetHealthScript.ApplyDamage(10);
                
            }
            
        }
        yield return new WaitForSeconds(0.05f);
        isShootingActive = false;
        lineRenderer.enabled = false;
    }

    public IEnumerator MakeFlamethrowerFlame(){
        GameObject bullet;
        Rigidbody2D rb;

        (bullet, rb) = CreateGenericBullet(2 * damageMultiplier, playerCritChance, playerCritMultiplier, 1, "flame", 0.5f, 0.75f, UnityEngine.Random.Range(-5, 5));

        yield return new WaitForSeconds(0.1f);

        isShootingActive = false;
    }

    private (GameObject, Rigidbody2D) CreateGenericBullet(
        float damage,
        float playerCritChance,
        float playerCritMultiplier,
        float size, 
        string spriteName, 
        float bulletLife = 0,
        float speedMultiplier = 1, 
        float rotationOffset = 0
        )
    {
        GameObject bullet = new GameObject(spriteName);

        var bulletDamage = damage * GameController.Instance.globalAttributes.globalPlayerBaseDamage;
        var num = UnityEngine.Random.value;

        var bulletScript = bullet.AddComponent<Bullet>();
        var isCritActive = playerCritChance < num;
        if(isCritActive){
            bulletScript.isCritBullet = true;
            bulletDamage *= playerCritMultiplier; 
        }
        
        bulletScript.power = bulletDamage;
        

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
