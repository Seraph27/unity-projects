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
}