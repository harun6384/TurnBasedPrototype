using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public RegionData curRegion;

    // Spawn points
    public string nextSpawnPoint;

    // Kahraman
    public GameObject heroCharacter;

    // Pozisyonlar
    public Vector3 nextHeroPosition;
    public Vector3 lastHeroPosition; // Savaþ

    // Sahneler
    public string sceneToLoad;
    public string lastScene; // Savaþ

    // Bools
    public bool isWalking = false;
    public bool canGetEncounter = false;
    public bool gotAttacked = false;

    // Enum
    public enum GameStates
    {
        WORLD_STATE,
        TOWN_STATE,
        BATTLE_STATE,
        IDLE
    }

    // Battle
    public int enemyAmount;
    public List<GameObject> enemyToBattle = new List<GameObject>();

    public GameStates gameState;

    private void Awake()
    {
        // Instance var mý kontrol et
        if (instance == null)
        {
            // Yoksa instance'yi buna ata
            instance = this;
        }
        // Varsa ama bu instance deðilse
        else if (instance != this)
        {
            // Yok et
            Destroy(gameObject);
        }
        // Bunu yok edilemez olarak ayarla
        DontDestroyOnLoad(gameObject);
        if (!GameObject.Find("HeroChar"))
        {
            GameObject Hero = Instantiate(heroCharacter, nextHeroPosition, Quaternion.identity) as GameObject;
            Hero.name = "HeroChar";
        }
    }

    private void Update()
    {
        switch (gameState)
        {
            case (GameStates.WORLD_STATE):
                if (isWalking)
                {
                    RandomEncounter();
                }
                if (gotAttacked)
                {
                    gameState = GameStates.BATTLE_STATE;

                }
                break;
            case (GameStates.TOWN_STATE):
                break;
            case (GameStates.BATTLE_STATE):
                // Savaþ sahnesini yükle
                StartBattle();
                // Ýdle'ye git
                gameState = GameStates.IDLE;
                break;
            case (GameStates.IDLE):
                break;
        }
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadSceneAfterBattle()
    {
        SceneManager.LoadScene(lastScene);
    }

    void RandomEncounter()
    {
        if (isWalking && canGetEncounter)
        {
            if (Random.Range(0,10000)<10)
            {
                gotAttacked = true;
            }
        }
    }

    void StartBattle()
    {
        // Düþmanlarýn sayýsý
        enemyAmount = Random.Range(1, curRegion.maxAmountEnemies + 1);
        // Hangi düþmanlar
        for (int i = 0; i < enemyAmount; i++)
        {
            enemyToBattle.Add(curRegion.possibleEnemies[Random.Range(0, curRegion.possibleEnemies.Count)]);
        }
        // Kahraman
        lastHeroPosition = GameObject.Find("HeroChar").gameObject.transform.position;
        nextHeroPosition = lastHeroPosition;
        lastScene = SceneManager.GetActiveScene().name;
        // Leveli yükle
        SceneManager.LoadScene(curRegion.BattleScene);
        // Kahramaný sýfýrla
        isWalking = false;
        gotAttacked = false;
        canGetEncounter = false;
    }
}
