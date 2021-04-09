using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailEnemy : EnemyController
{
    public GameObject poisonTrail; 
    protected PlayerController playerController;
    protected bool isActive = false;
    private List<Transform> playerWaypoints;
    private int mode = 0;
    private int wayPointIndex = 0;
    private float timer = 0;
    private float encircleSpeed = 20f;
    Coroutine co;
    
    override public int GetBaseHp(){
        return 200;
    }

    override protected void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        playerWaypoints = new List<Transform>();

        for(int i = 0; i < 4; i++){
            playerWaypoints.Add(playerController.transform.GetChild(i).transform);
        } 
        playerWaypoints.Add(playerController.transform.GetChild(0).transform);
        base.Start();
    }
    // Update is called once per frame
    override protected void Update()
    {
        var distanceToPlayer = (Vector2)(playerController.transform.position - transform.position);
        

        if(distanceToPlayer.magnitude < 5){
            isActive = true;
        }
        
        if(!isActive) return;

        if (mode == 0){  //follow
            timer += Time.deltaTime;
            wayPointIndex = 0;
             
            transform.position += (Vector3)(distanceToPlayer * 0.005f);
            //print(timer);
            if(distanceToPlayer.magnitude < 3 && timer > 5){
                mode = 1;
                trail();
            }
        }
            
        if(mode == 1){  //encircle
            var wayPointVec3 = playerWaypoints[wayPointIndex].position;
            float step = encircleSpeed   * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(wayPointVec3.x, wayPointVec3.y), step);
            
            if((transform.position - wayPointVec3).magnitude < 0.1f) {
                wayPointIndex++;
            }
            if(wayPointIndex == playerWaypoints.Count){
                mode = 0;
                timer = 0;
                stopTrail();
            }
        }

        base.Update();
    }

    void trail(){
        co = StartCoroutine(TrailCoroutine());
    } 

    void stopTrail(){
        StopCoroutine(co);
    }                                                      

    IEnumerator TrailCoroutine() {
        while(true){
            yield return new WaitForSeconds(0.05f);
            Instantiate(poisonTrail, transform.position, Quaternion.identity);
            
        }
    }

}