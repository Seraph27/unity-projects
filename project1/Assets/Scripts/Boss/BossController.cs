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
    public GameObject hpBarPrefab;
    protected GameObject hpBar;
    public HealthBar hpBarScript;
    public Phase currentPhase = Phase.Spike;
    public GameObject player;
    public GameObject spikePrefab;
    // Start is called before the first frame update
    List<(Vector3, TileBase)> passable;
    GameObject missileEnemyPrefab;
    Sprite fireSprite;
    Tilemap p;
    public TileBase burnedTiles;
    void Start()
    {
        hpBar = Instantiate(hpBarPrefab);
        hpBarScript = hpBar.GetComponent<HealthBar>();
        hpBarScript.Initalize(gameObject, 300); 
        player = GameController.Instance.player;
        passable = GameObject.FindObjectOfType<SpawnEntites>().getTilePositions();
        p = GameController.Instance.passable;
        missileEnemyPrefab = GameController.Instance.getPrefabByName("InterestingEnemy");
        GameController.Instance.spriteHolder.loadSpritesByName("fire");
        fireSprite = GameController.Instance.spriteHolder.getSpriteByName("fire");
        StartCoroutine(PhaseCoroutine()); 
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D c){  //player dealt dmg
        if (GameController.Instance.isWithPlayerBullet(c)) {
            hpBarScript.ApplyDamage(c.gameObject.GetComponent<Bullet>().power);
            Destroy(c.gameObject);
        }
    }

    IEnumerator PhaseCoroutine(){
        yield return new WaitForSeconds(1);
        for(int x = -5; x < 5; x++){
            for(int y = -5; y < 5; y++){
                var randomNum = Random.Range(0, 100);
                if(randomNum > 50){
                    GameController.Instance.changeTileAtPosition(p, transform.position + new Vector3(x, y, 0), burnedTiles);                
                } 
            }
        }
        
        yield return new WaitForSeconds(4);

        var spikeCoroutine = StartCoroutine(MakeSpikeCoroutine());
        while(hpBarScript.value > 200){
            yield return new WaitForSeconds(1/60);
        }
        StopCoroutine(spikeCoroutine);

        currentPhase = Phase.Minions;
        var minionCoroutine = StartCoroutine(MinionCoroutine());
        while(hpBarScript.value > 100){
            yield return new WaitForSeconds(1/60);
        }
        StopCoroutine(minionCoroutine);

        currentPhase = Phase.Attack;
        GetComponent<Animator>().SetBool("isPhase3", true);
        var attackCoroutine = StartCoroutine(AttackCoroutine());
        while(hpBarScript.value > 0){
            yield return new WaitForSeconds(1/60);
        }

        StopCoroutine(attackCoroutine);
        Destroy(hpBar);
        Destroy(gameObject);
    }

    IEnumerator MakeSpikeCoroutine(){
        while(true){
            var spike = Instantiate(spikePrefab, player.transform.position, Quaternion.identity);
            var spikeRenderer = spike.GetComponent<SpriteRenderer>();
            spikeRenderer.sprite = fireSprite;
            yield return new WaitForSeconds(3);
        }
    }



    IEnumerator MinionCoroutine(){
        while(true){
            var closeTile = passable.Where(x => (x.Item1 - transform.position).magnitude < 4).ToList();
            var randomIndex = Random.Range(0, closeTile.Count);
            Vector3 randomVec3 = closeTile[randomIndex].Item1;
            Instantiate(missileEnemyPrefab, randomVec3, Quaternion.identity);
            yield return new WaitForSeconds(3);
        }
    }

    IEnumerator AttackCoroutine(){
        yield return new WaitForSeconds(2);
        while(true){
            yield return new WaitForSeconds(1);
            for(int i = -135; i < 45; i+=6){
                CreateBossFlame(i);
            }
            yield return new WaitForSeconds(1);
            yield return new WaitForSeconds(2);
            yield return new WaitForSeconds(1);
            for(int i = 45; i < 225; i+=6){
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
        bullet.tag = "EnemyProjectile";
        ren.sprite = GameController.Instance.spriteHolder.getSpriteByName("flame"); 
        ren.sortingLayerName = "Projectiles";
        ren.sortingOrder = 0;
        rb.gravityScale = 0;
        circleCollider.isTrigger = true;
        circleCollider.radius = 1 / 10;
        bullet.transform.localScale = new Vector3(3, 3, 0);
        var rotationVector =  (Vector2)(Quaternion.Euler(0, 0, rotationDegrees) * Vector2.right);   
        bullet.transform.rotation = Quaternion.Euler(0, 0, rotationDegrees);
        bullet.transform.position = transform.position + (Vector3)(rotationVector * 1.0f);
        rb.velocity = rotationVector * 10;     
        GameObject.Destroy(bullet, 5);

    }
}
