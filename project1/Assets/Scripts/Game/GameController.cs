using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using UnityEngine.Tilemaps;

public class GameController : Singleton<GameController>
{
    Dictionary<string, Vector3> savedPositions = new Dictionary<string, Vector3>();
    public Dictionary<string, GameObject> prefabs;
    public SpriteHolder spriteHolder = new SpriteHolder();
    public GameObject player;
    List<WeaponKind> savedWeaponKinds;
    float savedHealth;
    float savedMaxHealth;
    PlayerController playerController;

    public void setupGame(){ //when loading a new scene
        
        prefabs = Resources.LoadAll<GameObject>("Prefabs").ToDictionary(go => go.name, go => go);
        
        SpawnEntites entitySpawner = GameObject.FindObjectOfType<SpawnEntites>();
        entitySpawner.playerPrefab = prefabs["Player"];
        player = entitySpawner.spawnPlayer();
        playerController = player.GetComponent<PlayerController>();
        GameController.Instance.spriteHolder.loadSpritesByName("weapons");    

        playerController.RestorePlayerState(savedWeaponKinds, savedHealth, savedMaxHealth);
        

        entitySpawner.spawnEnemies(); 
        string currentSceneName = SceneManager.GetActiveScene().name;
        Vector3 spawnPadLocation = GameObject.FindObjectOfType<SavePositionTile>().transform.position;

        if (savedPositions.ContainsKey(currentSceneName)) {
            player.transform.position = savedPositions[currentSceneName];
        } 
    }

    public void SavePlayerState(){
        savedWeaponKinds = playerController.weapons.Select(x => x.kind).ToList();
        savedHealth = playerController.hpBarScript.value;
        savedMaxHealth = playerController.hpBarScript.maxValue;
    } 

    public GameObject getPrefabByName(string name){
        if(prefabs.ContainsKey(name)){
            return prefabs[name];
        } else {
            Debug.Log("couldn't find prefab with name " + name + " typo?");
            throw new KeyNotFoundException();
        }
    }

    public void deleteTilesWithName(Tilemap mapName, string name){
        var tiles = new List<(Vector3, TileBase)>();
        BoundsInt bounds = mapName.cellBounds;
        for (int x = bounds.min.x; x < bounds.max.x; x++) {
            for (int y = bounds.min.y; y < bounds.max.y; y++) {                                                                               
                TileBase tile = mapName.GetTile(new Vector3Int(x, y, 0));
                if (tile != null) {
                    if(tile.name == name){
                        mapName.SetTile(new Vector3Int(x, y, 0), null);
                    }
                }
            }
        }
    }


    public void savePlayerPositionOnTransition(Vector3 pos){
        savedPositions[SceneManager.GetActiveScene().name] = pos;
    }

    public bool isWithPlayer(Collider2D c){
        return c.gameObject == player;
    }

    public bool isWithPlayerBullet(Collider2D c){
        return c.gameObject.tag == "PlayerProjectile";
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
