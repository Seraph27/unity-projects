using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameController : Singleton<GameController>
{
    Dictionary<string, Vector3> savedPositions = new Dictionary<string, Vector3>();

    public void setupGame(){ //when loading a new scene
        SpawnEntites entitySpawner = GameObject.FindObjectOfType<SpawnEntites>();
        GameObject player = entitySpawner.spawnPlayer();
        entitySpawner.spawnEnemies(); 
        string currentSceneName = SceneManager.GetActiveScene().name;
        Vector3 spawnPadLocation = GameObject.FindObjectOfType<SavePositionTile>().transform.position;

        if (currentSceneName != null && savedPositions.ContainsKey(currentSceneName)) {
            player.transform.position = savedPositions[currentSceneName];
        } 
    }

    public void savePlayerPositionOnTransition(Vector3 pos){
        savedPositions[SceneManager.GetActiveScene().name] = pos;
    }

}
