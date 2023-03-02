using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class GameController : MonoBehaviour
{
    public bool possessionOnDeath = false;

    public int possessionLimit = 1;

    public GameObject[] agentTypes;

    public GameObject initPlayerAgent;
    
    //public GameObject agentTypeA;

    public GameObject playerSpawn;

    public GameObject[] enemySpawns;

    public int agentCap = 1;
    public int maxEnemySpawnCount = 1;
    public float enemySpawnDelay = 5;

    private List<GameObject> liveAgents = new List<GameObject>();

    private float enemySpawnTimer = 0;
    private int enemySpawnsRemaining = 0;

    private Quaternion spawnRot = Quaternion.identity;

    public void AddAgent(GameObject agent)
    {
        liveAgents.Add(agent);
    }

    public void RemoveAgent(GameObject agent) 
    { 
        liveAgents.Remove(agent);
    }

    void Start()
    {
        // set enemy spawn limit
        enemySpawnsRemaining = maxEnemySpawnCount;

        // player needs to be spawned first
        if (playerSpawn != null)
        {
            Vector3 playerInitSpawnPos = playerSpawn.GetComponent<Transform>().position;

            // Player spawn
            GameObject initPlayer = Instantiate(initPlayerAgent, playerInitSpawnPos, spawnRot);
            AddAgent(initPlayer);
            if (initPlayer != null)
            {
                if (initPlayer != null && possessionOnDeath)
                {
                    initPlayer.GetComponent<AgentControl>().SetAsPlayer(possessionLimit);
                }
                else
                {
                    initPlayer.GetComponent<AgentControl>().SetAsPlayer();
                }
            }
            else
            {
                Debug.Log("Player reference not found.");
            }
        }
    }

    void SpawnEnemies()
    {
        // Enemy Spawn method 1
        // Only spawns one enemy per enemy spawn point
        // Only spawns one type of enemy
        if (enemySpawns.Length > 0 && maxEnemySpawnCount > 0)
        {
            foreach (GameObject spawnP in enemySpawns)
            {
                if (spawnP != null)
                {
                    GameObject newEnemy = Instantiate(
                        agentTypes[0], 
                        spawnP.GetComponent<Transform>().position, 
                        spawnRot);
                    AddAgent(newEnemy);

                    if (newEnemy != null)
                    {
                        newEnemy.GetComponent<AgentControl>().SetAsEnemy();

                    }
                    enemySpawnsRemaining--;
                }

                if (liveAgents.Count >= agentCap || enemySpawnsRemaining <= 0)
                {
                    break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemySpawnsRemaining > 0 && liveAgents.Count < agentCap)
        {
            enemySpawnTimer += Time.deltaTime;

            if (enemySpawnTimer >= enemySpawnDelay)
            {
                SpawnEnemies();

                enemySpawnTimer = 0;

                // stop spawning if player dies
            }
        }
        //Debug.Log(liveAgents.Count);
        // possibly disable script when all enemies spawn
        // to improve performance (repeated if checking)
    }
}
