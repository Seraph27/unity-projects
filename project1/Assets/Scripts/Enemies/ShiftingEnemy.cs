using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class ShiftingEnemy : EnemyController
{
    List<(Vector3, TileBase)> passable;
    override protected void Start() {
        base.Start();
        passable = GameObject.FindObjectOfType<SpawnEntites>().getTilePositions();
    }

    override public int GetBaseHp(){
        return 200;
    }
    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D c){ 

        if (GameController.Instance.isWithPlayerBullet(c)) {
            float triggerChance = Random.Range(0, 100);
            print(triggerChance);
            if(triggerChance > 50){
                var closeTile = passable.Where(x => (x.Item1 - transform.position).magnitude < 4).ToList();
                var randomIndex = Random.Range(0, closeTile.Count);
                Vector3 randomVec3 = closeTile[randomIndex].Item1;
                transform.position = randomVec3;
            }
            else{
                hpBarScript.ApplyDamage(c.gameObject.GetComponent<Bullet>().power, c.gameObject.GetComponent<Bullet>().isCritBullet); 
                Destroy(c.gameObject);
            }
        }
    }
}