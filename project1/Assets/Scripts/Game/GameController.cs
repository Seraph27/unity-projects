using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using UnityEngine.Tilemaps;
using System.IO;

public class GameController : Singleton<GameController>
{
    Dictionary<string, Vector3> savedPositions = new Dictionary<string, Vector3>();
    List<string> completedScenes = new List<string>();
    public Dictionary<string, GameObject> prefabs;
    public SpriteHolder spriteHolder = new SpriteHolder();
    public GameObject player;
    //these two are saved and restored across levels transitions
    List<WeaponKind> savedWeaponKinds; 
    float savedHealth;
    PlayerController playerController;
    public Tilemap notPassable; 
    public Tilemap passable;
    string gameDataPath;
    //public ShopController shopControllerScript;
    public GameObject mainCamera;

    ///////////////GLOBAL ATTRIBUTES/////////////////////
    public float globalPlayerMaxHealth = 200;
    public float globalPlayerBaseDamage = 2.0f;
    public int globalPlayerCurrency = 100;
    ////////////////////////////////////////////////////



    public void setupGame(){ //when loading a new game scene
        gameDataPath = Path.Combine(Application.persistentDataPath, "game_data.txt");

        if (System.IO.File.Exists(gameDataPath)) {
            StreamReader reader = new StreamReader(gameDataPath);
            globalPlayerMaxHealth = Int32.Parse(reader.ReadLine());
            globalPlayerBaseDamage = float.Parse(reader.ReadLine());
            globalPlayerCurrency = Int32.Parse(reader.ReadLine());
            
        } else{
            saveGlobalsToFile();
        }

        prefabs = Resources.LoadAll<GameObject>("Prefabs").ToDictionary(go => go.name, go => go);

        if(SceneManager.GetActiveScene().name.Contains("Level")){
            setupLevel();
        }
    }

    public void setupLevel(){
        SpawnEntites entitySpawner = GameObject.FindObjectOfType<SpawnEntites>();
        entitySpawner.playerPrefab = prefabs["Player"];
        entitySpawner.boringEnemyPrefab = prefabs["BoringEnemy"];
        entitySpawner.interestingEnemyPrefab = prefabs["InterestingEnemy"];
        entitySpawner.shiftingEnemyPrefab = prefabs["ShiftingEnemy"];
        entitySpawner.dashEnemyPrefab = prefabs["DashEnemy"];
        entitySpawner.trailEnemyPrefab = prefabs["TrailEnemy"];

        player = entitySpawner.spawnPlayer();
        playerController = player.GetComponent<PlayerController>();
        notPassable = GameObject.Find("NotPassable").GetComponent<Tilemap>();
        passable = GameObject.Find("Passable").GetComponent<Tilemap>();
        //shopControllerScript = GameObject.FindObjectOfType<ShopController>();
        mainCamera = GameObject.Find("Main Camera");
        GameController.Instance.spriteHolder.loadSpritesByName("weapons");    
        playerController.RestorePlayerState(savedWeaponKinds, savedHealth);
        

        entitySpawner.spawnEnemies(); 
        string currentSceneName = SceneManager.GetActiveScene().name;
        Vector3 spawnPadLocation = GameObject.FindObjectOfType<SavePositionTile>().transform.position;

        if (savedPositions.ContainsKey(currentSceneName)) {
            player.transform.position = savedPositions[currentSceneName];
        } 
        
        if(completedScenes != null && completedScenes.Contains(SceneManager.GetActiveScene().name)){
            deleteTilesWithName(notPassable, "tileset1_66");
        }
    }

    public void addCompletedScenes(String sceneName){
        completedScenes.Add(sceneName);
    }

    public void SavePlayerState(){
        savedWeaponKinds = playerController.weapons.Select(x => x.kind).ToList();
        savedHealth = playerController.hpBarScript.value;
    } 

    public void saveGlobalsToFile(){
            StreamWriter writer = new StreamWriter(gameDataPath);
            writer.WriteLine(globalPlayerMaxHealth.ToString());
            writer.WriteLine(globalPlayerBaseDamage.ToString());
             writer.WriteLine(globalPlayerCurrency.ToString());
            writer.Close();
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

    public void changeTileAtPosition(Tilemap mapName, Vector3 worldPosition, TileBase tileToChangeInto){
        mapName.SetTile(mapName.WorldToCell(worldPosition), tileToChangeInto);
    }
    

    public void savePlayerPositionOnTransition(Vector3 pos){
        savedPositions[SceneManager.GetActiveScene().name] = pos;
    }

    public bool isWithPlayer(Collider2D c){
        return c.gameObject == player;
    }

    public bool isWithEnemy(Collider2D c){
        return c.gameObject.tag == "Enemy";
    }

    public bool isWithPlayerBullet(Collider2D c){
        return c.gameObject.tag == "PlayerProjectile";
    }

    public bool isWithNotPassableTile(Collider2D c){
        return c.gameObject.name == "NotPassable";
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
