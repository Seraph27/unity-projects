using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.InputSystem;



public class PlayerController : MonoBehaviour
{
    public GameObject hpBarPrefab;
    public string upKey;
    public string rightKey;
    public string leftKey;
    public string downKey;
    Sprite front;
    Sprite side;
    Sprite back;
    Sprite[] weaponSheet;
    SpriteRenderer ren;
    public Rigidbody2D rb;  
    public GameObject bulletPrefab;
    public float bulletVelocity;
    public HealthBar hpBarScript;
    public GameObject cashTextPrefab;
    public GameObject weaponIconPrefab;
    public GameObject iconFramePrefab;
    public bool isSlotAActive = true;
    Vector3 playerVelocity;
    GameObject weaponIconA;
    GameObject weaponIconB;
    public GameObject iconFrame;
    public List<Weapon> weapons = new List<Weapon>();
    public int activeWeaponIndexA = 0;
    public int activeWeaponIndexB = 1;
    public GameObject weaponDropPrefab;
    GameObject flamethrower;
    public int enemyKills; //kill for each scene used to unlock next lvl
    public GameObject exitGamePanelPrefab;
    GameObject onScreenHealthBarPrefab;
    OnScreenHealthBarController onScreenHealthBarScript;
    PlayerWeaponController playerWeaponController;
    public Animator animator;
    GameObject joystickPrefab;
    GameObject joystickRPrefab;
    GameObject andriodUI1;
    GameObject andriodUI2;
    //TESTING NEW INPUT
    Vector2 inputMovement;

    // Start is called before the first frame update
    void Init()
    {
        var hpBar = Instantiate(hpBarPrefab);
        hpBarScript = hpBar.GetComponent<HealthBar>();
        playerWeaponController = GetComponent<PlayerWeaponController>();
        GameController.Instance.spriteHolder.loadSpritesByName("playerSprites");
        GameController.Instance.spriteHolder.loadSpritesByName("flame");  //flamethrower flame
        GameController.Instance.spriteHolder.loadSpritesByName("bullet");
        GameController.Instance.spriteHolder.loadSpritesByName("weapons");
        GameController.Instance.spriteHolder.loadSpritesByName("weaponpack2");
        front = GameController.Instance.spriteHolder.getSpriteByName("frontView");
        side = GameController.Instance.spriteHolder.getSpriteByName("sideView");
        back = GameController.Instance.spriteHolder.getSpriteByName("backView"); 
        ren = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        iconFrame = Instantiate(iconFramePrefab, transform.position, Quaternion.identity);
        weaponIconA = Instantiate(weaponIconPrefab, transform.position, Quaternion.identity);
        weaponIconB = Instantiate(weaponIconPrefab, transform.position, Quaternion.identity);
        weaponIconB.GetComponentInChildren<Image>().transform.position += new Vector3(Screen.width * 0.07f, 0, 0);
        
        onScreenHealthBarPrefab = GameController.Instance.getPrefabByName("OnScreenHealth");
        Instantiate(onScreenHealthBarPrefab, Vector3.zero, Quaternion.identity);

        if(Application.platform == RuntimePlatform.Android){
            joystickPrefab = GameController.Instance.getPrefabByName("newjoystick");
            Instantiate(joystickPrefab, Vector3.zero, Quaternion.identity);
            joystickRPrefab = GameController.Instance.getPrefabByName("newjoystickR");
            Instantiate(joystickRPrefab, Vector3.zero, Quaternion.identity);
            andriodUI1 = GameController.Instance.getPrefabByName("andriodUI");
            andriodUI2 = GameController.Instance.getPrefabByName("andriodUI2");
            Instantiate(joystickRPrefab, Vector3.zero, Quaternion.identity);
            Instantiate(joystickRPrefab, Vector3.zero, Quaternion.identity);
        }
        
        
    }

    public void RestorePlayerState(List<WeaponKind> savedWeaponKinds, float savedHealth)
    {
        Init();

        if(savedWeaponKinds == null) {
            savedWeaponKinds = new List<WeaponKind>();  //starting weapons here
            savedWeaponKinds.Add(WeaponKind.PiuPiuLaser); 
            savedWeaponKinds.Add(WeaponKind.Laser);
        }
        if(savedHealth == 0){
            savedHealth = GameController.Instance.globalAttributes.globalPlayerMaxHealth;
        }


        foreach(var weaponKind in savedWeaponKinds){
            weapons.Add(Weapon.make_weapon(weaponKind, playerWeaponController));
            
        }
        weaponIconA.GetComponentInChildren<Image>().sprite = this.weapons[activeWeaponIndexA].icon;
        weaponIconB.GetComponentInChildren<Image>().sprite = this.weapons[activeWeaponIndexB].icon;

        hpBarScript.Initalize(gameObject, savedHealth, GameController.Instance.globalAttributes.globalPlayerMaxHealth);
    }

    void FixedUpdate(){
        rb.velocity = playerVelocity * GameController.Instance.globalAttributes.globalPlayerSpeed * Time.fixedDeltaTime;
    }
    
    public void OnMovement(InputAction.CallbackContext value)
    {
        inputMovement = value.ReadValue<Vector2>();
        Debug.Log(inputMovement);
    }
    

    bool isWeaponSwap = false;
    public void OnWeaponSwap(InputAction.CallbackContext value) //swapping with hand items
    {
        if(value.started){
            isWeaponSwap = true;
        }
    }

    bool isSwappingGround = false;
    public void OnSwapGround(InputAction.CallbackContext value)  //swappign with ground items
    {
        if(value.started){
            isSwappingGround = true;
        }
    }

    bool isPause = false;
    public void OnPause(InputAction.CallbackContext value)  //swappign with ground items
    {
        if(value.started){
            isPause = true;
        }
    }
    // Update is called once per frames
    void Update()
    {
        playerVelocity = new Vector3(inputMovement.x, inputMovement.y, 0);
        animator.SetFloat("playerSpeedX", playerVelocity.x);
        animator.SetFloat("playerSpeedY", playerVelocity.y);

        playerVelocity.Normalize();


        if(isWeaponSwap){
            isWeaponSwap = false;
            var screenWidth = Screen.width;
            var iconFramePos = iconFrame.transform.GetChild(0).transform;
            if(isSlotAActive){
                iconFramePos.position += new Vector3(screenWidth * 0.07f, 0, 0);
            } else{
                iconFramePos.position -= new Vector3(screenWidth * 0.07f, 0, 0);
            }

            isSlotAActive = !isSlotAActive;
        }

        //weapon swapping
        if(isSwappingGround){ 
            isSwappingGround = false;
            var weaponList = GameObject.FindGameObjectsWithTag("Weapon");        //this or use a collider onTrigger to check. 
            GameObject closestWeapon = null;
            float closestDistance = Mathf.Infinity;
            Vector3 position = transform.position;
 
            foreach (GameObject go in weaponList)
            {
                Vector3 diff = go.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < closestDistance)
                {
                    closestWeapon = go;
                    closestDistance = curDistance;
                }
            }
            
            if(closestWeapon != null && closestDistance <= 4){
                var closestWeaponType = closestWeapon.GetComponent<GunDropController>().gunType;

                if(isSlotAActive){
                    Weapon.make_drop(transform.position, weapons[activeWeaponIndexA].kind);
                    weapons[0] = Weapon.make_weapon(closestWeaponType, playerWeaponController);
                    weaponIconA.GetComponentInChildren<Image>().sprite = weapons[activeWeaponIndexA].icon;

                } else{
                    Weapon.make_drop(transform.position, weapons[activeWeaponIndexB].kind);
                    weapons[1] = Weapon.make_weapon(closestWeaponType, playerWeaponController);
                    weaponIconB.GetComponentInChildren<Image>().sprite = weapons[activeWeaponIndexB].icon;
                } 

                GameObject.Destroy(closestWeapon);
            }
        }

        if(isPause){
            isPause = false;
            Debug.Log("escaped");
            gameObject.SetActive(false);
            var exitGamePanel = Instantiate(exitGamePanelPrefab, Vector3.zero, Quaternion.identity);
            var c = exitGamePanel.AddComponent<CameraFollowScript>();
            c.depth = 0;
        }

        if(!hpBarScript.IsAlive()){   //player dies
            GameController.Instance.playAudio("DiedBG");
            string currentSceneName = SceneManager.GetActiveScene().name;  //this stops music 
            GameController.Instance.stopAudio(currentSceneName + "BG");
            SceneManager.LoadScene("EndScene"); 
        }

    }

    void OnTriggerEnter2D(Collider2D c) { 
        //boring enemy bombs //should remove sometime this is not good
        if (c.gameObject.tag == "BoringEnemyAttack") {
            hpBarScript.ApplyDamage(10); 
            Destroy(c.gameObject);
        }

        if(c.gameObject.tag == "DragonFire"){
            hpBarScript.ApplyDamage(50); 
            Destroy(c.gameObject);
        }
    }
}
