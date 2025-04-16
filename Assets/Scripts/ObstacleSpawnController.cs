using UnityEngine;
using UnityEngine.VFX;

public class ObstacleSpawnController : MonoBehaviour
{
    //code referenced from Muddy Wolf https://www.youtube.com/watch?v=vClEQ1GqMPw&list=PLfX6C2dxVyLylMufxTi7DM9Vjlw5bff1c&index=3
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private Transform obstacleParent;
    public float obstacleSpawnTime = 2f;
    public float obstacleSpeed = 1f;

    private float timeUntilObstacleSpawn;

    private void Start()
    {
        LevelManager.Instance.onGameOver.AddListener(ClearObstacles);
    }

    private void Update()
    {
        if (LevelManager.Instance.isPlaying)
        {
            ObstacleSpawnLoop();
        }        
    }

    private void ObstacleSpawnLoop() //can set the timer to a random range
    {
        timeUntilObstacleSpawn += Time.deltaTime;

        if (timeUntilObstacleSpawn >= obstacleSpawnTime)
        {
            ObstacleSpawn();
            timeUntilObstacleSpawn = 0f;
        }
    }

    private void ClearObstacles()
    {
        foreach(Transform child in obstacleParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void ObstacleSpawn()
    {
        GameObject obstacleToSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        GameObject spawnedObstacle = Instantiate(obstacleToSpawn, transform.position, Quaternion.identity);
        spawnedObstacle.transform.parent = obstacleParent;

        Rigidbody2D obstacleRB = spawnedObstacle.GetComponent<Rigidbody2D>();
        obstacleRB.linearVelocity = Vector2.left * obstacleSpeed;
    }
}
