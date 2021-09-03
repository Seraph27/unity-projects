using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.InputSystem;


public enum WeaponKind{
    PiuPiuLaser,
    Shotgun,
    Flamethrower,
    Laser,
    GrenadeLauncher,

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
        {WeaponKind.Laser, "weapons_16"},
        {WeaponKind.GrenadeLauncher, "weapons_6"}
    };
    public string audioName;
    public bool isAudioContinous;

    public static Weapon make_weapon(WeaponKind kind, PlayerWeaponController playerWeaponController){
        if(kind == WeaponKind.PiuPiuLaser){
            return new Weapon(WeaponKind.PiuPiuLaser, 25, weaponIcons[kind], playerWeaponController.MakePiuPiuBullet, "PistolSoundEffect", false);
        }   else if(kind == WeaponKind.Shotgun){
            return new Weapon(WeaponKind.Shotgun, 10, weaponIcons[kind], playerWeaponController.MakeShotgunBlast, "ShotgunSoundEffect", false);
        }   else if(kind == WeaponKind.Flamethrower){
            return new Weapon(WeaponKind.Flamethrower, 2, weaponIcons[kind], playerWeaponController.MakeFlamethrowerFlame, "FlamethrowerSoundEffect2", true);
        }   else if(kind == WeaponKind.Laser){
            return new Weapon(WeaponKind.Laser, 10, weaponIcons[kind], playerWeaponController.MakeLaserBeam, "LaserSoundEffect", true);
        }   else if(kind == WeaponKind.GrenadeLauncher){
            return new Weapon(WeaponKind.GrenadeLauncher, 10, weaponIcons[kind], playerWeaponController.MakeGrenadeLauncher, "GrenadeLaunchSoundEffect", false);
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

    public Weapon(WeaponKind kind, int damage, string iconName, Func<IEnumerator> makeBulletFunc, string audioName, bool isAudioContinous) {
        this.kind = kind;
        this.damage = damage;
        this.icon = GameController.Instance.spriteHolder.getSpriteByName(iconName);
        this.makeBulletFunc = makeBulletFunc;
        this.audioName = audioName;
        this.isAudioContinous = isAudioContinous;
    }
}

public class PlayerWeaponController : MonoBehaviour
{
    GameObject linePrefab;
    LineRenderer lineRenderer;
    PlayerController playerController;
    float playerCritChance;
    float playerCritMultiplier;
    float damageMultiplier = 1.0f; 
    bool isShootingActive = false;
    GameObject grenadePrefab;
    bool isAudioOn; //also filters out if audio is continous (like laser / flamethrower)
    bool isAttacking = false;
    Vector3 mousePosition;
    Vector2 joystickAttack;  //for aiming relative to joystick's position

    private void Start() {
        linePrefab = GameController.Instance.getPrefabByName("LineLaser");
        lineRenderer = Instantiate(linePrefab, Vector3.zero, Quaternion.identity).GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        playerController = GetComponent<PlayerController>();
        playerCritChance = GameController.Instance.globalAttributes.globalPlayerCritChance;
        playerCritMultiplier = GameController.Instance.globalAttributes.globalPlayerCritMultiplier;
        grenadePrefab = GameController.Instance.getPrefabByName("Grenade");
    }
    
    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.started){
            isAttacking = true;  
        }
        if(context.canceled){
            isAttacking = false;
        }
    } 

    
    public void OnJoystickAttack(InputAction.CallbackContext context)
    {
        if(context.started){
            isAttacking = true;   
        }
        if(context.canceled){
            isAttacking = false;
        }
        joystickAttack = context.ReadValue<Vector2>();
    }   

    public void OnAim(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }   
    
    private void Update() {
        if (isAttacking && !isShootingActive) {
            isShootingActive = true;          
            StartCoroutine(playerController.weapons[playerController.currentWeaponIndex].makeBulletFunc()); 
            if(playerController.weapons[playerController.currentWeaponIndex].isAudioContinous && !isAudioOn){
                isAudioOn = true;
                GameController.Instance.playAudio(playerController.weapons[playerController.currentWeaponIndex].audioName);
                GameController.Instance.startAudioLoop(playerController.weapons[playerController.currentWeaponIndex].audioName);
            }   
        } 

        if(!isAttacking && isShootingActive && isAudioOn && playerController.weapons[playerController.currentWeaponIndex].isAudioContinous){
            isAudioOn = false;
            GameController.Instance.stopAudio(playerController.weapons[playerController.currentWeaponIndex].audioName);
            GameController.Instance.stopAudioLoop(playerController.weapons[playerController.currentWeaponIndex].audioName);
        }
    }

    public IEnumerator MakePiuPiuBullet()
    {
        GameObject bullet;
        Rigidbody2D rb;
        GameController.Instance.playAudio("PistolSoundEffect");

        (bullet, rb) = CreateGenericBullet(25 * damageMultiplier, playerCritChance, playerCritMultiplier, 1, "bullet", 2, 1, 0, "BulletExplosion", 0.75f);
        yield return new WaitForSeconds(0.33f);

        isShootingActive = false;

    }

    public IEnumerator MakeShotgunBlast() {
        GameObject bullet;
        Rigidbody2D rb;
        GameController.Instance.playAudio("ShotgunSoundEffect"); 

        for (int i = 0; i < 10; i++) {
            (bullet, rb) = CreateGenericBullet(10 * damageMultiplier, playerCritChance, playerCritMultiplier, 1, "bullet", 0.5f, 2, UnityEngine.Random.Range(-30, 30), "BulletExplosion", 0.3f);
        }
        yield return new WaitForSeconds(0.75f);

        isShootingActive = false;
    }

    public IEnumerator MakeLaserBeam(){
        if(!isAudioOn){
            isAudioOn = true;
            GameController.Instance.playAudio("LaserSoundEffect");
            GameController.Instance.startAudioLoop("LaserSoundEffect");
        }
               
        var worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);  //bullet shooting
        var direction = (Vector3)(worldMousePos - transform.position) * 10;      
        Debug.DrawRay(transform.position, direction, Color.red, 1f);

        lineRenderer.enabled = true;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, 100f);
        
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
                targetHealthScript.ApplyDamage(20);
            }
        }
        yield return new WaitForSeconds(0.05f);

        lineRenderer.enabled = false;
        isShootingActive = false;

    }

    public IEnumerator MakeFlamethrowerFlame(){
        GameObject bullet;
        Rigidbody2D rb;
        if(!isAudioOn){
            isAudioOn = true;
            GameController.Instance.playAudio("FlamethrowerSoundEffect2");
            GameController.Instance.startAudioLoop("FlamethrowerSoundEffect2");
        }
     
        (bullet, rb) = CreateGenericBullet(10 * damageMultiplier, playerCritChance, playerCritMultiplier, 1, "flame", 0.5f, 0.75f, UnityEngine.Random.Range(-5, 5), "FlameExplosion");

        yield return new WaitForSeconds(0.1f);

        isShootingActive = false;
    }

    public IEnumerator MakeGrenadeLauncher(){
        GameObject bullet;
        Rigidbody2D rb;
        GameController.Instance.playAudio("GrenadeLaunchSoundEffect"); 

        (bullet, rb) = CreateGrenade(10 * damageMultiplier, playerCritChance, playerCritMultiplier, 2, "bullet", 2, 0.75f);
        
        yield return new WaitForSeconds(1);
        isShootingActive = false;
    }


   
    private (GameObject, Rigidbody2D) CreateGenericBullet(
        float damage,
        float playerCritChance,
        float playerCritMultiplier,
        float size, 
        string spriteName,                //REMEMBER TO HAVE SPRITE READY
        float bulletLife = 0,
        float speedMultiplier = 1, 
        float rotationOffset = 0,
        string particleEffectName = "MeteorExplosion",
        float particleScale = 1
        )
    {
        GameObject bulletParent = new GameObject("bulletParent"); //setup
        GameObject bullet = new GameObject(spriteName);
        bullet.transform.parent = bulletParent.transform;

        GameObject explosionPrefab = GameController.Instance.getPrefabByName(particleEffectName);  //particle effect
        var explosionObject = Instantiate(explosionPrefab, bulletParent.transform.position, Quaternion.identity);
        explosionObject.transform.parent = bulletParent.transform;
        var explosion = explosionObject.GetComponent<ParticleSystem>();
        explosionObject.transform.localScale *= particleScale;

        var bulletDamage = damage * GameController.Instance.globalAttributes.globalPlayerBaseDamage; //add damage
        var num = UnityEngine.Random.value;

        var bulletScript = bullet.AddComponent<Bullet>();
        var isCritActive = playerCritChance < num;
        if(isCritActive){
            bulletScript.isCritBullet = true;
            bulletDamage *= playerCritMultiplier; 
        }
        
        bulletScript.power = bulletDamage;
        

        var ren = bullet.AddComponent<SpriteRenderer>();                                   //bullet setup 
        Rigidbody2D rb = bulletParent.AddComponent<Rigidbody2D>();
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
        //EnumerableHelper.bulletRotationAndVelocityJoystick(transform, bulletParent.transform, rotationOffset, rb, playerController.attackJoystickScript);
        bulletRotationAndVelocity(transform, bulletParent.transform, rotationOffset, rb, mousePosition);

        if(bulletLife > 0){
            GameObject.Destroy(bulletParent, bulletLife);
        }
        return (bullet, rb);
    }


    private (GameObject, Rigidbody2D) CreateGrenade(
        float damage,
        float playerCritChance,
        float playerCritMultiplier,
        float size, 
        string spriteName,                //REMEMBER TO HAVE SPRITE READY
        float bulletLife = 0,
        float speedMultiplier = 1, 
        float rotationOffset = 0
        )
    {
        var grenade = Instantiate(grenadePrefab, transform.position, Quaternion.identity);
        var grenadeScript = grenade.GetComponent<Grenade>();
        var bulletDamage = damage * GameController.Instance.globalAttributes.globalPlayerBaseDamage;
        var num = UnityEngine.Random.value;
        var isCritActive = playerCritChance < num;
        if(isCritActive){
            grenadeScript.isCritBullet = true;
            bulletDamage *= playerCritMultiplier; 
        }
        
        grenadeScript.power = bulletDamage;
        var rb = grenade.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        //EnumerableHelper.bulletRotationAndVelocity(transform, grenade.transform, rotationOffset, rb);
        
        if(bulletLife > 0){
            GameObject.Destroy(grenade, bulletLife);
        }
        return (grenade, rb);

    }



    public void bulletRotationAndVelocity(Transform playerTransform, Transform bulletTransform, float rotationOffset, Rigidbody2D rb, Vector3 mousePos, float bulletSpeedMultiplier = 1){
        Vector2 direction;
        if(Application.platform == RuntimePlatform.Android){
            direction = joystickAttack;
        } else{
            var worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);  //bullet shooting
            direction = (Vector2)(worldMousePos - playerTransform.position);
        }
       
        direction.Normalize();
        var rotationDegrees = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + rotationOffset;
        var rotationVector =  (Vector2)(Quaternion.Euler(0, 0, rotationDegrees) * Vector2.right);
        bulletTransform.rotation = Quaternion.Euler(0, 0, rotationDegrees);
        bulletTransform.position = playerTransform.position + (Vector3)(rotationVector * 1.0f);
        rb.velocity = rotationVector * 10 * bulletSpeedMultiplier;
    }

}
