using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterestingEnemy : EnemyController
{

    override public int GetBaseHp(){
        return 200;
    }
    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
    }

    protected override void OnDied()
    {
        base.OnDied();
        var chance = Random.value;
        if(chance > 0.1f){
            Weapon.make_drop(transform.position, WeaponKind.Flamethrower);
        }  
    }
}