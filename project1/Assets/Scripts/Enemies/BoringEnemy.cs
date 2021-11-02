using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction{
    Up,
    Down,
    Left,
    Right
}

public class BoringEnemy : EnemyController
{
    Direction direction = Direction.Right;
    float totalDistance = 0;
    Vector3 oldPos;
    protected override void Start()
    {
        base.Start();
        oldPos = transform.position;

    }

    override public int GetBaseHp(){
        return 200;
    }
    // Update is called once per frame
    override protected void Update()
    {
        Vector3 distanceVector = transform.position - oldPos;
        float distanceThisFrame = distanceVector.magnitude;
        totalDistance += distanceThisFrame;
        oldPos = transform.position;
        if(totalDistance > 5){
            totalDistance = 0;
            
        }

        rb.velocity = velocity * 100 *  Time.deltaTime;

        base.Update();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        direction *= -1;
    }


}
