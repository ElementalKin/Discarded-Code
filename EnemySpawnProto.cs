using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class EnemySpawnProto : MonoBehaviour
{
    public int BiomeStage = 0;
    public SceneManagment sm;

    private int randomEnemy1;
    private int randomEnemy2;
    public TextMeshProUGUI TEMPTEXTREMOVEMELATER;

    public bool spawnOnStart = true;

    public Transform EnemySpawnPoint;
    public Transform SelectionEnemySpawnPoint1;
    public Transform SelectionEnemySpawnPoint2;
    //public List <GameObject> BiomeEnemies11 = new List<GameObject>();
    //public List <GameObject> BiomeEnemies12 = new List<GameObject>();
    //public List <GameObject> BiomeEnemies21 = new List<GameObject>();
    //public List <GameObject> BiomeEnemies22 = new List<GameObject>();
    //public GameObject Boss;
    public List <GameObject> SelectionBiomeEnemies11 = new List<GameObject>();
    public List <GameObject> SelectionBiomeEnemies12 = new List<GameObject>();
    public List <GameObject> SelectionBiomeEnemies21 = new List<GameObject>();
    public List <GameObject> SelectionBiomeEnemies22 = new List<GameObject>();
    public GameObject SelectionBoss;
    [HideInInspector]
    public List <GameObject> SelectionEnemyPrefabsCopy = new List<GameObject>();


    public Image enemyIntentImage;
    public StateMachine stateMachine;
    public CardFunctionality cardPlayer;
    public PartSelectionUI endOfBattleUI;

    public GameObject battleUI;

    public StateMachine state;

    private List<EnemyAI> activeEnemies;

    //private List<ClickedOn> BiomeEnemiesClickable;

    readonly System.Random rand = new System.Random();

    private ClickedOn enemySpawn1;
    private ClickedOn enemySpawn2;

    private void Start()
    {
        // Makes a copy of the selection list for looping
        //for (int i = 0; i < SelectionEnemyPrefabs.Count; i++)
        //{
        //    SelectionEnemyPrefabsCopy.Add(SelectionEnemyPrefabs[i]);
        //}

        activeEnemies = new List<EnemyAI>();
        //BiomeEnemiesClickable = new List<ClickedOn>();


        //if (spawnOnStart)
        //{
        //    SpawnEnemy(Random.Range(0, enemyPrefabs.Length));
        //}

        //battleUI.SetActive(false);

        ClickEnemy.rayCastOn = true;

        SpawnEnemySelection();
    }

    public void SpawnEnemySelection()
    {
        //Debug.Log("sm number of battles = "+ SceneManagment.numberOfBattles + "  state number of battles = "+state.numberOfBattle);
        //sm.numberOfBattles = FindObjectOfType<SceneManagment>().numberOfBattles
        //sm.numberOfBattles = state.numberOfBattle;
        battleUI.SetActive(false);
        //ClickEnemy.rayCastOn = true;
        //TEMP That loops AI even after completing biome
        //if (SelectionEnemyPrefabs.Count <= 1)
        //{
        //    SelectionEnemyPrefabs.Clear();
        //    for (int i = 0; i < SelectionEnemyPrefabsCopy.Count; i++)
        //    {
        //        SelectionEnemyPrefabs.Add(SelectionEnemyPrefabsCopy[i]);
        //    }
        //}
        //if (true)
        //{
        //    sm.numberOfBattles = state.numberOfBattle;

        //}
        switch (SceneManagment.numberOfBattles)
        {
            case 0:
            case 1:
                //Debug.Log("First Biome");
                randomEnemy1 = rand.Next(0, SelectionBiomeEnemies11.Count);
                randomEnemy2 = rand.Next(0, SelectionBiomeEnemies11.Count);
                if (randomEnemy1 == randomEnemy2)
                {
                    randomEnemy2 += 1;
                    if (randomEnemy2 > SelectionBiomeEnemies11.Count - 1)
                    {
                        randomEnemy2 = 0;
                    }
                }
                enemySpawn1 = Instantiate(SelectionBiomeEnemies11[randomEnemy1], SelectionEnemySpawnPoint1).GetComponent<ClickedOn>();
                enemySpawn1.isEnemy1 = true;
                enemySpawn1.gameObject.SetActive(true);
                enemySpawn2 = Instantiate(SelectionBiomeEnemies11[randomEnemy2], SelectionEnemySpawnPoint2).GetComponent<ClickedOn>();
                enemySpawn2.isEnemy1 = false;
                enemySpawn2.gameObject.SetActive(true);
                break;
            case 2:
            case 3:
                //Debug.Log("First Biome 2");
                randomEnemy1 = rand.Next(0, SelectionBiomeEnemies12.Count);
                randomEnemy2 = rand.Next(0, SelectionBiomeEnemies12.Count);
                if (randomEnemy1 == randomEnemy2)
                {
                    randomEnemy2 += 1;
                    if (randomEnemy2 > SelectionBiomeEnemies12.Count - 1)
                    {
                        randomEnemy2 = 0;
                    }
                }
                enemySpawn1 = Instantiate(SelectionBiomeEnemies12[randomEnemy1], SelectionEnemySpawnPoint1).GetComponent<ClickedOn>();
                enemySpawn1.isEnemy1 = true;
                enemySpawn1.gameObject.SetActive(true);
                enemySpawn2 = Instantiate(SelectionBiomeEnemies12[randomEnemy2], SelectionEnemySpawnPoint2).GetComponent<ClickedOn>();
                enemySpawn2.isEnemy1 = false;
                enemySpawn2.gameObject.SetActive(true);
                break;
            case 4:
                //Debug.Log("sm load level case 4;");
                if (SceneManagment.isSceneLoaded != true)
                {
                    SceneManagment.isSceneLoaded = true;
                    SceneManagment.playerHealth = cardPlayer.GetComponent<Deck>().health;
                    sm.loadLevel(cardPlayer.playerPartManager);
                }
                else
                {
                    // Add the old parts onto the Qin(?)
                    for (int i = 0; i < SceneManagment.oldParts.allParts.Length; i++)
                    {
                        if (SceneManagment.oldParts.allParts[i] != null)
                        {
                            cardPlayer.playerPartManager.ReplacePart(i, SceneManagment.oldParts.allParts[i]);

                            cardPlayer.playerPartManager.AddSticker(SceneManagment.oldParts.allPartsComponents[i].sticker, i);
                        }
                    }
                    
                    cardPlayer.GetComponent<Deck>().health = SceneManagment.playerHealth;

                    Destroy(SceneManagment.oldParts.gameObject);
                    // End add parts -------------------
                }

                //Debug.Log("Second Biome");
                randomEnemy1 = rand.Next(0, SelectionBiomeEnemies21.Count);
                randomEnemy2 = rand.Next(0, SelectionBiomeEnemies21.Count);
                if (randomEnemy1 == randomEnemy2)
                {
                    randomEnemy2 += 1;
                    if (randomEnemy2 > SelectionBiomeEnemies21.Count - 1)
                    {
                        randomEnemy2 = 0;
                    }
                }
                enemySpawn1 = Instantiate(SelectionBiomeEnemies21[randomEnemy1], SelectionEnemySpawnPoint1).GetComponent<ClickedOn>();
                enemySpawn1.isEnemy1 = true;
                enemySpawn1.gameObject.SetActive(true);
                enemySpawn2 = Instantiate(SelectionBiomeEnemies21[randomEnemy2], SelectionEnemySpawnPoint2).GetComponent<ClickedOn>();
                enemySpawn2.isEnemy1 = false;
                enemySpawn2.gameObject.SetActive(true);
                break;
            case 5:
                //sm.numberOfBattles = state.numberOfBattle;
                //Debug.Log("Second Biome");
                SceneManagment.isSceneLoaded = false;
                randomEnemy1 = rand.Next(0, SelectionBiomeEnemies21.Count);
                randomEnemy2 = rand.Next(0, SelectionBiomeEnemies21.Count);
                if (randomEnemy1 == randomEnemy2)
                {
                    randomEnemy2 += 1;
                    if (randomEnemy2 > SelectionBiomeEnemies21.Count - 1)
                    {
                        randomEnemy2 = 0;
                    }
                }
                enemySpawn1 = Instantiate(SelectionBiomeEnemies21[randomEnemy1], SelectionEnemySpawnPoint1).GetComponent<ClickedOn>();
                enemySpawn1.isEnemy1 = true;
                enemySpawn1.gameObject.SetActive(true);
                enemySpawn2 = Instantiate(SelectionBiomeEnemies21[randomEnemy2], SelectionEnemySpawnPoint2).GetComponent<ClickedOn>();
                enemySpawn2.isEnemy1 = false;
                enemySpawn2.gameObject.SetActive(true);
                break;
            case 6:
            case 7:
                //Debug.Log("Second Biome2");
                randomEnemy1 = rand.Next(0, SelectionBiomeEnemies22.Count);
                randomEnemy2 = rand.Next(0, SelectionBiomeEnemies22.Count);
                if (randomEnemy1 == randomEnemy2)
                {
                    randomEnemy2 += 1;
                    if (randomEnemy2 > SelectionBiomeEnemies22.Count - 1)
                    {
                        randomEnemy2 = 0;
                    }
                }
                enemySpawn1 = Instantiate(SelectionBiomeEnemies22[randomEnemy1], SelectionEnemySpawnPoint1).GetComponent<ClickedOn>();
                enemySpawn1.isEnemy1 = true;
                enemySpawn1.gameObject.SetActive(true);
                enemySpawn2 = Instantiate(SelectionBiomeEnemies22[randomEnemy2], SelectionEnemySpawnPoint2).GetComponent<ClickedOn>();
                enemySpawn2.isEnemy1 = false;
                enemySpawn2.gameObject.SetActive(true);
                break;
            case 8:
                if (SceneManagment.isSceneLoaded != true)
                {
                    SceneManagment.isSceneLoaded = true;
                    SceneManagment.playerHealth = cardPlayer.GetComponent<Deck>().health;
                    sm.loadLevel(cardPlayer.playerPartManager);
                }
                else
                {
                    // Add the old parts onto the Qin(?)
                    for (int i = 0; i < SceneManagment.oldParts.allParts.Length; i++)
                    {
                        if (SceneManagment.oldParts.allParts[i] != null)
                        {
                            cardPlayer.playerPartManager.ReplacePart(i, SceneManagment.oldParts.allParts[i]);

                            cardPlayer.playerPartManager.AddSticker(SceneManagment.oldParts.allPartsComponents[i].sticker, i);
                        }
                    }

                    cardPlayer.GetComponent<Deck>().health = SceneManagment.playerHealth;

                    Destroy(SceneManagment.oldParts.gameObject);
                    // End add parts -------------------
                }

                enemySpawn1 = Instantiate(SelectionBoss, SelectionEnemySpawnPoint1).GetComponent<ClickedOn>();
                enemySpawn1.isEnemy1 = true;
                enemySpawn1.gameObject.SetActive(true);
                enemySpawn2 = Instantiate(SelectionBoss, SelectionEnemySpawnPoint2).GetComponent<ClickedOn>();
                enemySpawn2.isEnemy1 = false;
                enemySpawn2.gameObject.SetActive(true);
                //state.beginNewCombat();
                //Debug.Log("boss");
                break;
            default:
                Debug.Log("Switch case Default in enemy Spawn Proto, SpawnEnemySelection");
                break;
        }

        //SelectionEnemyPrefabs.RemoveAt(randomEnemy1);
        //SelectionEnemyPrefabs.RemoveAt(randomEnemy2);

    }

    public void DeleteClickableLeftovers()
    {
        Destroy(enemySpawn1.gameObject);
        Destroy(enemySpawn2.gameObject);
    }

    public void SpawnEnemy(GameObject ai, GameObject clickable)
    {
        battleUI.SetActive(true);

        switch (SceneManagment.numberOfBattles)
        {
            case 0:
            case 1:
                //Debug.Log("Case 0 1 Delete enemy");
                for (int i = 0; i < SelectionBiomeEnemies11.Count; i++)
                {
                    if (clickable.name == string.Format("{0}(Clone)", SelectionBiomeEnemies11[i].name))
                    {
                        Destroy(enemySpawn1.gameObject);
                        Destroy(enemySpawn2.gameObject);
                        SelectionBiomeEnemies11.RemoveAt(i);
                    }
                }
                break;
            case 2:
            case 3:
                //Debug.Log("Case 2 3 Delete enemy");

                for (int i = 0; i < SelectionBiomeEnemies12.Count; i++)
                {
                    if (clickable.name == string.Format("{0}(Clone)", SelectionBiomeEnemies12[i].name))
                    {
                        Destroy(enemySpawn1.gameObject);
                        Destroy(enemySpawn2.gameObject);
                        SelectionBiomeEnemies12.RemoveAt(i);
                    }
                }
                break;
            case 4:
            case 5:
                //Debug.Log("Case 4 5 Delete enemy");

                for (int i = 0; i < SelectionBiomeEnemies21.Count; i++)
                {
                    if (clickable.name == string.Format("{0}(Clone)", SelectionBiomeEnemies21[i].name))
                    {
                        Destroy(enemySpawn1.gameObject);
                        Destroy(enemySpawn2.gameObject);
                        SelectionBiomeEnemies21.RemoveAt(i);
                    }
                }
                break;
            case 6:
            case 7:
                for (int i = 0; i < SelectionBiomeEnemies22.Count; i++)
                {
                    if (clickable.name == string.Format("{0}(Clone)", SelectionBiomeEnemies22[i].name))
                    {
                        Destroy(enemySpawn1.gameObject);
                        Destroy(enemySpawn2.gameObject);
                        SelectionBiomeEnemies22.RemoveAt(i);
                    }
                }
                break;
            case 8:
                //Debug.Log("boss");
                break;
            default:
                Debug.Log(" Default in enemy spawn proto SpawnEnemy");
                break;
        }
        //for (int i = 0; i < SelectionEnemyPrefabs.Count; i++)
        //{
        //    if (clickable.name == string.Format("{0}(Clone)", SelectionEnemyPrefabs[i].name))
        //    {
        //        Destroy(enemySpawn1.gameObject);
        //        Destroy(enemySpawn2.gameObject);
        //        SelectionEnemyPrefabs.RemoveAt(i);
        //    }
        //}

        EnemyAI newEnemy = Instantiate(ai, EnemySpawnPoint).GetComponent<EnemyAI>();

        //newEnemy.display = enemyIntentImage;
        newEnemy.state = stateMachine;
        newEnemy.enemyDeck.TEMPTEXTREMOVEMELATER = this.TEMPTEXTREMOVEMELATER;
        newEnemy.cardPlayer = cardPlayer;
        activeEnemies.Add(newEnemy);
    }


    public void RemoveEnemies()
    {
        battleUI.SetActive(false);

        //Debug.Log("Remove Enemies Called");
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            Destroy(activeEnemies[i].gameObject);
            activeEnemies.RemoveAt(i);
        }
    }
}
