using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameController : Singleton<GameController>
{
    Dictionary<string, Vector3> savedPositions = new Dictionary<string, Vector3>();
    public Dictionary<string, GameObject> prefabs;
    public SpriteHolder spriteHolder = new SpriteHolder();
    public GameObject player;

    public void setupGame(){ //when loading a new scene

        prefabs = Resources.LoadAll<GameObject>("Prefabs").ToDictionary(go => go.name, go => go);
        
        SpawnEntites entitySpawner = GameObject.FindObjectOfType<SpawnEntites>();
        entitySpawner.playerPrefab = prefabs["Player"];
        player = entitySpawner.spawnPlayer();
        entitySpawner.spawnEnemies(); 
        string currentSceneName = SceneManager.GetActiveScene().name;
        Vector3 spawnPadLocation = GameObject.FindObjectOfType<SavePositionTile>().transform.position;

        if (savedPositions.ContainsKey(currentSceneName)) {
            player.transform.position = savedPositions[currentSceneName];
        } 
    }

    public void savePlayerPositionOnTransition(Vector3 pos){
        savedPositions[SceneManager.GetActiveScene().name] = pos;
    }

    public bool isWithPlayer(Collider2D c){
        return c.gameObject == player;
    }

   

}

public class SpriteHolder{
    List<Sprite> sprites = new List<Sprite>();
    public void loadSpritesByName(string path){
        Sprite[] newSprites = Resources.LoadAll<Sprite>("Sprites/" + path);
        foreach (Sprite sprite in newSprites) {
            if (!sprites.Any(s => s.name == sprite.name)) {
                sprites.Add(sprite);
                // Debug.Log($"Adding {sprite.name} to sprite holder.");
            }
        }
    }
 

    public Sprite getSpriteByName(string spriteName){
        return sprites.Single(s => s.name == spriteName);
    }
}
