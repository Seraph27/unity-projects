using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameController : Singleton<GameController>
{
    Dictionary<string, Vector3> savedPositions = new Dictionary<string, Vector3>();
    public Dictionary<string, GameObject> prefabs;
    public SpriteHolder spriteHolder = new SpriteHolder();

    public void setupGame(){ //when loading a new scene

        prefabs = Resources.LoadAll<GameObject>("Prefabs").ToDictionary(go => go.name, go => go);
        
        SpawnEntites entitySpawner = GameObject.FindObjectOfType<SpawnEntites>();
        entitySpawner.playerPrefab = prefabs["Player"];
        GameObject player = entitySpawner.spawnPlayer();
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

   

}

public class SpriteHolder{
    List<Sprite> sprites = new List<Sprite>();
    public void loadSpritesByName(string path){
        Sprite[] spriteArr = Resources.LoadAll<Sprite>("Sprites/" + path);
        // foreach (Sprite sprite in spriteArr) {
        //     if (!sprites.Contains(x => x.name == sprite.name)) {
                
        //     }
        // }
        Debug.Log(string.Join(",", sprites.Select(s => s.name)));
        sprites.AddRange(spriteArr);
    }
 

    public Sprite getSpriteByName(string spriteName){
        return sprites.First(s => s.name == spriteName);
    }
}
