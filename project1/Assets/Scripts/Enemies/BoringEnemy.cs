using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MovingDirection{
    Up,
    Down,
    Left,
    Right,
}

public class BoringEnemy : EnemyController
{
    int direction = 1;
    GameObject coinDrop;
    MovingDirection movingDirection = MovingDirection.Right;

    override public int GetBaseHp(){
        return 200;
    }

    protected override void Start()
    {
        coinDrop = GameController.Instance.getPrefabByName("coinDrop");
        base.Start();
    }
    // Update is called once per frame
    override protected void Update()
    {
        Vector3 velocity = new Vector3(0, 0, 0);

        if(movingDirection == MovingDirection.Up){
            velocity = new Vector3(0, 1, 0);
        } else if(movingDirection == MovingDirection.Down){
            velocity = new Vector3(0, -1, 0);
        } else if(movingDirection == MovingDirection.Right){
            velocity = new Vector3(1, 0, 0);
        } else if(movingDirection == MovingDirection.Left){
            velocity = new Vector3(-1, 0, 0);
        }
        
        rb.velocity = velocity * 100 *  Time.deltaTime;

        base.Update();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        MovingDirection newMovingDirection = movingDirection;
        while(newMovingDirection == movingDirection){
            newMovingDirection = (MovingDirection)Random.Range(0, System.Enum.GetValues(typeof(MovingDirection)).Length);
        }
        movingDirection = newMovingDirection;
    }

    protected override void OnDied()
    {
        base.OnDied();
        Instantiate(coinDrop, transform.position, Quaternion.identity);        
    }
}
