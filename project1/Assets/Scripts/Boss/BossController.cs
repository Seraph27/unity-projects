using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public enum Phase{
    Spike,
    Minions,
    Attack,
}

public class BossController : MonoBehaviour
{
    // public GameObject hpBarPrefab;
    // protected GameObject hpBar;
    // public HealthBar hpBarScript;
    public Phase currentPhase = Phase.Spike;
    public GameObject player;
    public GameObject spikePrefab;
    // Start is called before the first frame update
    List<(Vector3, TileBase)> passableTiles;
    GameObject missileEnemyPrefab;
    Tilemap passable;
    public TileBase burnedTiles;
    GameObject bossHealthBar;
    public GameObject bossHpBarPrefab;
    protected GameObject bossHpBar;
    public BossHealthBar bossHpBarScript;
    public GameObject portal;

    void Start()
    {
        player = GameController.Instance.player;
        passableTiles = GameObject.FindObjectOfType<SpawnEntites>().getTilePositions();
        passable = GameController.Instance.passable;
        missileEnemyPrefab = GameController.Instance.getPrefabByName("InterestingEnemy");
        portal = GameController.Instance.getPrefabByName("portal");

        //Screen Health Bar
        bossHpBarPrefab = GameController.Instance.getPrefabByName("bossHealthBarParent");
        bossHpBar = Instantiate(bossHpBarPrefab, Vector3.zero, Quaternion.identity);
        bossHpBar.name = "bossHpBar";
        var bossHpBarChild = bossHpBar.transform.GetChild(0).gameObject;
        bossHpBarScript = bossHpBarChild.AddComponent<BossHealthBar>();
        bossHpBarScript.Initalize(5000);

        var cameraFollowScript = bossHpBar.AddComponent<CameraFollowScript>();
        cameraFollowScript.depth = 1;

        StartCoroutine(PhaseCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if(passable.GetTile(Vector3Int.FloorToInt(player.transform.position)).name == "ProjectUtumno_full_384"){
            Debug.Log("ahhhh");
        }
    }

    void OnTriggerEnter2D(Collider2D c){  //player dealt dmg
        if (GameController.Instance.isWithPlayerBullet(c)) {
            bossHpBarScript.ApplyDamage(c.gameObject.GetComponent<Bullet>().power, c.gameObject.GetComponent<Bullet>().isCritBullet);
            Destroy(c.gameObject);
        }
        Debug.Log(c.gameObject.name);
    }

    IEnumerator PhaseCoroutine(){
        yield return new WaitForSeconds(1);
        for(int x = -14; x <= 14; x++){
            for(int y = -14; y <= 13; y++){
                var randomNum = Random.Range(0, 100);
                if(randomNum > 85){
                    GameController.Instance.changeTileAtPosition(passable, transform.position + new Vector3(x, y, 0), burnedTiles);                
                } 
            }
        }
        
        yield return new WaitForSeconds(4);

        var spikeCoroutine = StartCoroutine(MakeSpikeCoroutine());
        while(bossHpBarScript.value > bossHpBarScript.maxValue * 0.75f){
            yield return new WaitForSeconds(1/60);
        }
        StopCoroutine(spikeCoroutine);

        currentPhase = Phase.Minions;
        var minionCoroutine = StartCoroutine(MinionCoroutine());
        while(bossHpBarScript.value > bossHpBarScript.maxValue * 0.5f){
            yield return new WaitForSeconds(1/60);
        }
        StopCoroutine(minionCoroutine);

        currentPhase = Phase.Attack;
        GetComponent<Animator>().SetBool("isPhase3", true);
        var attackCoroutine = StartCoroutine(AttackCoroutine());
        while(bossHpBarScript.value > 0){
            yield return new WaitForSeconds(1/60);
        }

        StopCoroutine(attackCoroutine);

        GameController.Instance.swapTilesWithName(GameController.Instance.notPassable, "tileset1_68");
        var potral = Instantiate(portal, new Vector3(-1.5f, 33.5f, 0), Quaternion.identity);
        potral.AddComponent<EndScreenController>();
        Destroy(bossHpBar);
        Destroy(gameObject);
    }

    IEnumerator MakeSpikeCoroutine(){
        while(true){
            for(int i = 0; i < 1; i++){
                var randomVec3 = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                Instantiate(spikePrefab, player.transform.position + randomVec3, Quaternion.identity);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }



    IEnumerator MinionCoroutine(){
        while(true){
            var closeTile = passableTiles.Where(x => (x.Item1 - transform.position).magnitude > 4 && (x.Item1 - transform.position).magnitude < 8).ToList();
            var randomIndex = Random.Range(0, closeTile.Count);
            Vector3 randomVec3 = closeTile[randomIndex].Item1;
            var bossMinions = Instantiate(missileEnemyPrefab, randomVec3, Quaternion.identity);
            bossMinions.transform.localScale *= 1.5f;
            bossMinions.GetComponent<HomingAttack>().missileRange = 10;
            yield return new WaitForSeconds(3);
        }
    }

    IEnumerator AttackCoroutine(){
        yield return new WaitForSeconds(2);
        while(true){
            yield return new WaitForSeconds(1);
            for(int i = -135; i < 45; i+=8){
                CreateBossFlame(i);
            }
            yield return new WaitForSeconds(1);
            yield return new WaitForSeconds(2);
            yield return new WaitForSeconds(1);
            for(int i = 45; i < 225; i+=8){
                CreateBossFlame(i);
            }
            yield return new WaitForSeconds(1);
            yield return new WaitForSeconds(2);
        }
    }

    void CreateBossFlame(int rotationDegrees){
        GameObject bullet = new GameObject("bossFlame");
        bullet.AddComponent<Bullet>().power = 10;
        var ren = bullet.AddComponent<SpriteRenderer>();
        Rigidbody2D rb = bullet.AddComponent<Rigidbody2D>();
        var circleCollider = bullet.AddComponent<CircleCollider2D>();
        bullet.tag = "DragonFire";
        ren.sprite = GameController.Instance.spriteHolder.getSpriteByName("flame"); 
        ren.sortingLayerName = "Projectiles";
        ren.sortingOrder = 0;
        rb.gravityScale = 0;
        circleCollider.isTrigger = true;
        circleCollider.radius = 0.2f;
        circleCollider.offset = new Vector2(0.2f, 0);
        bullet.transform.localScale = new Vector3(3, 3, 0);
        var rotationVector =  (Vector2)(Quaternion.Euler(0, 0, rotationDegrees) * Vector2.right);   
        bullet.transform.rotation = Quaternion.Euler(0, 0, rotationDegrees);
        bullet.transform.position = transform.position + (Vector3)(rotationVector * 1.0f);
        rb.velocity = rotationVector * 10;     
        GameObject.Destroy(bullet, 5);

    }
}
