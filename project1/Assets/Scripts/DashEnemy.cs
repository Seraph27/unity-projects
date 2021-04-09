using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum DashEnemyMode{
    Follow,
    Dash,
}

public class DashEnemy : EnemyController
{
    protected PlayerController playerController;
    protected bool isActive = false;
    private DashEnemyMode mode = DashEnemyMode.Follow;
    private float timer = 0;
    Vector2 dashDirection;

    override public int GetBaseHp(){
        return 200;
    }

    override protected void Start()
    {

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        base.Start();
    }
    // Update is called once per frame
    override protected void Update()
    {
        var distanceToPlayer = (Vector2)(playerController.transform.position - transform.position);
        

        if(distanceToPlayer.magnitude < 5){
            isActive = true;
        }

        if(hpBarScript.value < hpBarScript.maxValue){
            isActive = true;
        }
        
        if(!isActive) return;

        if (mode == DashEnemyMode.Follow){ 
            timer += Time.deltaTime;

        
            transform.position += (Vector3)(distanceToPlayer * 0.005f);

            if(distanceToPlayer.magnitude < 3 && timer > 5){
                mode = DashEnemyMode.Dash;
                dashDirection = distanceToPlayer;
            }
        } 
            
        if(mode == DashEnemyMode.Dash){ 
            transform.position += (Vector3)(dashDirection * 0.1f);
        }

        base.Update();
    }
}