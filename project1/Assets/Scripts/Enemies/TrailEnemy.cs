using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum TrailEnemyMode{
    Follow,
    Encircle,
}

public class TrailEnemy : EnemyController
{
    public GameObject poisonTrail; 
    protected bool isActive = false;
    private List<Transform> playerWaypoints;
    private TrailEnemyMode mode = TrailEnemyMode.Follow;
    private int wayPointIndex = 0;
    private float timer = 0;
    private float encircleSpeed = 20f;
    List<Transform> reorderedList;
    Coroutine co;
    
    override public int GetBaseHp(){
        return 200;
    }

    override protected void Start()
    {
        base.Start();
        playerWaypoints = new List<Transform>();

        for(int i = 0; i < 4; i++){
            playerWaypoints.Add(playerController.transform.GetChild(i).transform);
        } 
        playerWaypoints.Add(playerController.transform.GetChild(0).transform);
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

        if (mode == TrailEnemyMode.Follow){  //follow
            wayPointIndex = 0;
            timer += Time.deltaTime;

            Transform nearestWaypoint = playerWaypoints.OrderBy(waypoint => (transform.position - waypoint.position).magnitude).First();
            int nearestIndex = playerWaypoints.IndexOf(nearestWaypoint);
            reorderedList = 
                playerWaypoints.Skip(nearestIndex).Take(playerWaypoints.Count - nearestIndex)
                .Concat(
                    playerWaypoints.Take(nearestIndex)
                ).ToList();

            transform.position += (Vector3)(distanceToPlayer * 0.005f);
            //print(timer);
            if(distanceToPlayer.magnitude < 3 && timer > 5){
                mode = TrailEnemyMode.Encircle;
                trail();
            }
        } 
            
        if(mode == TrailEnemyMode.Encircle){  //encircle
            var wayPointVec3 = reorderedList[wayPointIndex].position;
            float step = encircleSpeed   * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(wayPointVec3.x, wayPointVec3.y), step);
            
            if((transform.position - wayPointVec3).magnitude < 0.1f) {
                wayPointIndex++;
            }
            if(wayPointIndex == reorderedList.Count){
                mode = TrailEnemyMode.Follow;
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