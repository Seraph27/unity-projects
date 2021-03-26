using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailEnemy : EnemyController
{
    protected PlayerController playerController;
    protected bool isActive = false;
    private List<Transform> playerWaypoints;
    private int waypointIndex = 0;
    
    override public int GetBaseHp(){
        return 200;
    }

    override protected void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        playerWaypoints = new List<Transform>() {
            // get waypoint 1,
            // get waypoint 2,
            // ...
            // get waypoint 1
        };
        // playerWaypoints.Add()
        // length = playerWaypoints.Count
        
        base.Start();
    }
    // Update is called once per frame
    override protected void Update()
    {
        var distanceToPlayer = (Vector2)(playerController.transform.position - transform.position);

        base.Update();
        if(distanceToPlayer.magnitude < 5){
            isActive = true;
        }
        
        if(!isActive) return;

        // if mode = following
        transform.position += (Vector3)(distanceToPlayer * 0.005f);
        
        // if mode = encircling
        // move towards playerWaypoint[waypointIndex]
        // if dist( playerWaypoint[waypointIndex], me) < 0.1f:
        //    waypointIndex++
        // if last index, then mode = following
        


    }

    void trail(){


    }

}