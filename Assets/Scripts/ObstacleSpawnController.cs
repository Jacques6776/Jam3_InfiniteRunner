using UnityEngine;
using UnityEngine.VFX;

public class ObstacleSpawnController : MonoBehaviour
{
    //code referenced from Muddy Wolf https://www.youtube.com/watch?v=vClEQ1GqMPw&list=PLfX6C2dxVyLylMufxTi7DM9Vjlw5bff1c&index=3
    //Sets up spawning and time between spawns
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private Transform obstacleParent;
    public float obstacleSpawnTime = 2f;
    public float obstacleSpeed = 1f;

    //Setting up difficulty curve
    [Range(0, 1)] public float obstacleSpawnTimeFactor = 0.1f;
    [Range(0, 1)] public float obstacleSpeedFactor = 0.2f;

    private float currentObstacleSpawnTime;
    private float currentObstacleSpeed;

    private float timeAlive; //record how long the player has been alive

    private float timeUntilObstacleSpawn;

    [SerializeField] private LevelManager levelManager;

    [SerializeField] private bool gameInProgress = false;

    private void Start()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
    }

    private void Update()
    {        
        if (gameInProgress)
        {            
            CalculateFactors();
            ObstacleSpawnLoop();
        }
        else
        {
            ClearObstacles();
            ResetFactors();
        }
    }

    //Switch to start spawners
    public void ActivateSpawner(bool gameState)
    {
        gameInProgress = gameState;
    }

    public void DeactivateSpawner(bool gameState)
    {
        gameInProgress = gameState;
    }

    //Spawn controls
    private void ObstacleSpawnLoop() //can set the timer to a random range
    {
        timeUntilObstacleSpawn += Time.deltaTime;

        if (timeUntilObstacleSpawn >= currentObstacleSpawnTime)
        {
            ObstacleSpawn();
            timeUntilObstacleSpawn = 0f;
        }
    }

    private void CalculateFactors() // ups the difficulty by an equation
    {
        Debug.Log("Hey");
        currentObstacleSpawnTime = obstacleSpawnTime / Mathf.Pow(timeAlive, obstacleSpawnTimeFactor);
        currentObstacleSpeed = obstacleSpeed * Mathf.Pow(timeAlive, obstacleSpeedFactor);
    }

    private void ObstacleSpawn()//Spawns obstacles
    {
        GameObject obstacleToSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        GameObject spawnedObstacle = Instantiate(obstacleToSpawn, transform.position, Quaternion.identity);
        spawnedObstacle.transform.parent = obstacleParent;

        Rigidbody2D obstacleRB = spawnedObstacle.GetComponent<Rigidbody2D>();
        obstacleRB.linearVelocity = Vector2.left * currentObstacleSpeed;
    }

    private void ClearObstacles() //clears all obstacles on death
    {
        foreach (Transform child in obstacleParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void ResetFactors() // resets the game to its base factors upon game over / restart
    {
        timeAlive = 1f;
        currentObstacleSpawnTime = obstacleSpawnTime;
        currentObstacleSpeed = obstacleSpeed;
    }
}
