using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorEnemy : EnemyController
{
    override public int GetBaseHp(){
        return 300;
    }
    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
    }
}
