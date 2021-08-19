using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using UnityEngine.Tilemaps;
using System.IO;
using TMPro;

public class GlobalAttributes{
    public float globalPlayerMaxHealth = 200;
    public float globalPlayerBaseDamage = 2.0f;
    public float globalPlayerCritChance = 0.5f;
    public float globalPlayerCritMultiplier = 1.5f;
    public int globalPlayerSpeed = 400;
    public int globalPlayerCurrency = 100;

    public int globalPlayerMaxHealthLevel = 0;
    public int globalPlayerBaseDamageLevel = 0;
    public int globalPlayerSpeedLevel = 0;
}

    
public class GameController : Singleton<GameController>
{
    Dictionary<string, Vector3> savedPositions = new Dictionary<string, Vector3>();
    List<string> completedScenes = new List<string>();
    public Dictionary<string, GameObject> prefabs;
    public Dictionary<string, AudioClip> audioClips;
    public Dictionary<string, AudioSource> audioSources;
    public GameObject audioParent;
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
    public GameObject globalLight2D;
    public Dictionary<string, string> levelDesc;

    public void setupGame(){ //when loading a new game scene
        gameDataPath = Path.Combine(Application.persistentDataPath, "game_data.txt");

        Debug.Log(gameDataPath);
        if (System.IO.File.Exists(gameDataPath)) {
            string str = File.ReadAllText(gameDataPath);
            globalAttributes = JsonUtility.FromJson<GlobalAttributes>(str);
        } else{
            globalAttributes = new GlobalAttributes();
            saveGlobalsToFile(); 
        }

        prefabs = Resources.LoadAll<GameObject>("Prefabs").ToDictionary(go => go.name, go => go);
        audioClips = Resources.LoadAll<AudioClip>("Audio").ToDictionary(clip => clip.name, clip => clip);
        

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

        if(audioParent == null){
            
            setupAudio();
        }

        levelDesc = new Dictionary<string, string>();
        levelDesc["Level1"] = "Grassy Lands";  //level desc
        //Debug.Log(levelDesc["Level1"]);


        if(SceneManager.GetActiveScene().name.Contains("Level")){
            
            setupLevel();
        }

        globalLight2D = getPrefabByName("Global Light 2D");  
        Instantiate(globalLight2D, Vector3.zero, Quaternion.identity);

        
    }

    public void setupLevel(){
        SpawnEntites entitySpawner = GameObject.FindObjectOfType<SpawnEntites>();
        entitySpawner.playerPrefab = prefabs["Player"];
        entitySpawner.boringEnemyPrefab = prefabs["BoringEnemy"];
        entitySpawner.interestingEnemyPrefab = prefabs["InterestingEnemy"];
        entitySpawner.shiftingEnemyPrefab = prefabs["ShiftingEnemy"];
        entitySpawner.dashEnemyPrefab = prefabs["DashEnemy"];
        entitySpawner.trailEnemyPrefab = prefabs["TrailEnemy"];
        entitySpawner.meteorEnemyPrefab = prefabs["MeteorEnemy"];
        entitySpawner.tankEnemyPrefab = prefabs["TankEnemy"];

        player = entitySpawner.spawnPlayer();
        playerController = player.GetComponent<PlayerController>();
        notPassable = GameObject.Find("NotPassable").GetComponent<Tilemap>();
        passable = GameObject.Find("Passable").GetComponent<Tilemap>();
        //shopControllerScript = GameObject.FindObjectOfType<ShopController>();
        mainCamera = GameObject.Find("Main Camera");
        mainCamera.GetComponent<Camera>().orthographicSize = 7;
        GameController.Instance.spriteHolder.loadSpritesByName("weapons");    
        playerController.RestorePlayerState(savedWeaponKinds, savedHealth);

        //enter level desc
        string currentSceneName = SceneManager.GetActiveScene().name;
        var levelEntryTextPrefab = getPrefabByName("LevelEntryText");
        var levelEntryText = Instantiate(levelEntryTextPrefab, transform.position, Quaternion.identity);
        levelEntryText.GetComponentInChildren<TextMeshProUGUI>().SetText(levelDesc[currentSceneName]);

        entitySpawner.spawnEnemies(); 
        
        if (savedPositions.ContainsKey(currentSceneName)) {
            player.transform.position = savedPositions[currentSceneName];
        } 
        
        if((completedScenes != null && completedScenes.Contains(SceneManager.GetActiveScene().name)) || portalOpensAtStart){
            swapTilesWithName(notPassable, "tileset1_66");
        }
    }

    public void setupAudio(){
        audioSources = new Dictionary<string, AudioSource>();
        audioParent = new GameObject("AudioParent");
        audioParent.transform.parent = gameObject.transform;

        void makeAudio(string audioName) {
            GameObject soundEffectChild = new GameObject(audioName);
            audioSources[audioName] = soundEffectChild.AddComponent<AudioSource>();
            audioSources[audioName].clip = audioClips[audioName];
            audioSources[audioName].playOnAwake = false;
            soundEffectChild.transform.parent = audioParent.transform;
        }

        makeAudio("PistolSoundEffect");
        makeAudio("ShotgunSoundEffect");
        
    }

    public void playAudio(string audioName){
        audioSources[audioName].Play();
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
