using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using UnityEngine.Tilemaps;
using System.IO;

public class GlobalAttributes{
    public float globalPlayerMaxHealth = 200;
    public float globalPlayerBaseDamage = 2.0f;
    public float globalPlayerCritChance = 0.5f;
    public float globalPlayerCritMultiplier = 1.5f;
    public int globalPlayerSpeed = 400;
    public int globalPlayerCurrency = 100;
   
}

    
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
    public GlobalAttributes globalAttributes;
    public List<string> levelNames;
    public Dictionary<string, List<string>> allLevels;
    public int currentLevelIndex;
    public bool portalOpensAtStart = true;
    public static int totalEnemyKills;

    public void setupGame(){ //when loading a new game scene
        gameDataPath = Path.Combine(Application.persistentDataPath, "game_data.txt");

        Debug.Log(gameDataPath);
        if (System.IO.File.Exists(gameDataPath)) {
            string str = File.ReadAllText(gameDataPath);
            globalAttributes = JsonUtility.FromJson<GlobalAttributes>(str);
            Debug.Log(globalAttributes.globalPlayerBaseDamage);

        } else{
            globalAttributes = new GlobalAttributes();
            saveGlobalsToFile(); 
        }

        prefabs = Resources.LoadAll<GameObject>("Prefabs").ToDictionary(go => go.name, go => go);

        allLevels = new Dictionary<string, List<string>>();
        allLevels["lvl1"] = new List<string>(){"Level1"};
        allLevels["lvl2"] = new List<string>(){"Level2A", "Level2B"};
        allLevels["lvl3"] = new List<string>(){"Level3A", "Level3B"};
        allLevels["lvl4"] = new List<string>(){"Level4A", "Level4B"};
        allLevels["boss"] = new List<string>(){"Level5"};
        
        if(levelNames == null){
            levelNames = new List<string>();              
            levelNames.Add(allLevels["lvl1"].RandomElement());
            // levelNames.Add(allLevels["lvl2"].RandomElement());
            // levelNames.Add(allLevels["lvl3"].RandomElement());
            // levelNames.Add(allLevels["lvl4"].RandomElement());
            levelNames.Add(allLevels["boss"].RandomElement());
            Debug.Log(string.Join(", ", levelNames));
        } 

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
        mainCamera.GetComponent<Camera>().orthographicSize = 7;
        GameController.Instance.spriteHolder.loadSpritesByName("weapons");    
        playerController.RestorePlayerState(savedWeaponKinds, savedHealth);

        

        entitySpawner.spawnEnemies(); 
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (savedPositions.ContainsKey(currentSceneName)) {
            player.transform.position = savedPositions[currentSceneName];
        } 
        
        if((completedScenes != null && completedScenes.Contains(SceneManager.GetActiveScene().name)) || portalOpensAtStart){
            swapTilesWithName(notPassable, "tileset1_66");
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
        File.WriteAllText(gameDataPath, JsonUtility.ToJson(globalAttributes, true));
    }

    public GameObject getPrefabByName(string name){
        if(prefabs.ContainsKey(name)){
            return prefabs[name];
        } else {
            Debug.Log("couldn't find prefab with name " + name + " typo?");
            throw new KeyNotFoundException();
        }
    }

    public void swapTilesWithName(Tilemap mapName, string name, TileBase tileToChangeInto = null){ //default will be delete
        BoundsInt bounds = mapName.cellBounds;
        for (int x = bounds.min.x; x < bounds.max.x; x++) {
            for (int y = bounds.min.y; y < bounds.max.y; y++) {                                                                               
                TileBase tile = mapName.GetTile(new Vector3Int(x, y, 0));
                if (tile != null) {
                    if(tile.name == name){
                        mapName.SetTile(new Vector3Int(x, y, 0), tileToChangeInto);
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
