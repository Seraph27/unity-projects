using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum DashEnemyMode{
    Follow,
    Preparing,
    Dash,
}

public class DashEnemy : EnemyController
{
    protected PlayerController playerController;
    protected bool isActive = false;
    private DashEnemyMode mode = DashEnemyMode.Follow;
    private float timer = 0;
    Vector2 dashDirection;
    private bool hasAppliedDamage = false;

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
        timer += Time.deltaTime;

        if (mode == DashEnemyMode.Follow){ 
            

            

        
            transform.position += (Vector3)(distanceToPlayer * 0.005f);

            if(distanceToPlayer.magnitude < 3 && timer > 5){
                timer = 0;
                mode = DashEnemyMode.Preparing;
                dashDirection = distanceToPlayer;
            }
        } 
        
        if(mode == DashEnemyMode.Preparing){ 
            ren.enabled = (int)(timer * 10) % 2 == 0;

            if(timer > 1){
                ren.enabled = true;
                timer = 0;
                hasAppliedDamage = false;
                mode = DashEnemyMode.Dash;
            }
        }

        if(mode == DashEnemyMode.Dash){ 
            transform.position += (Vector3)(dashDirection.normalized * 0.1f);
            if(distanceToPlayer.magnitude < 0.5 && !hasAppliedDamage){
                playerController.rb.AddForce(dashDirection.normalized * 2000);   // decouple camera and update camnera controlelealea
                playerController.hpBarScript.ApplyDamage(20);
                hasAppliedDamage = true;
            }

            /*
                function easeInOutCubic(x: number): number {
                    return x < 0.5 ? 4 * x * x * x : 1 - pow(-2 * x + 2, 3) / 2;
                }
            */
            if(distanceToPlayer.magnitude > 7){
                timer = 0;
                mode = DashEnemyMode.Follow; 
                
            }
        }

        base.Update();
    }
}