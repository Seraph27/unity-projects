using UnityEngine;

public class GameController : Singleton<GameController>
{
    public int i;
    public Vector3 level1Pos; //dictionary scenename: pos

    public void setupGame(){
        SpawnEntites entitySpawner = GameObject.FindObjectOfType<SpawnEntites>();
        GameObject player = entitySpawner.spawnPlayer();
        entitySpawner.spawnEnemies(); //get player and put it in right place

        player.transform.position = level1Pos;
    }

    public void savePlayerPositionOnTransition(Vector3 pos){
        level1Pos = pos;
    }

    // override protected void Awake(){
    //     base.Awake();
    //     setupGame();
    // }
}
