using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [SerializeField] private int clearObsticlePoint = 1;
    [SerializeField] private int chargeObstaclePoint = 2;

    public LevelManager levelManager;

    private void Awake()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "ChargeState")
        {
            levelManager.IncreaseGameScore(chargeObstaclePoint);
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Despawner"))
        {
            Destroy(gameObject);
        }

        if(collision.CompareTag("Player"))
        {
            levelManager.IncreaseGameScore(clearObsticlePoint);
            levelManager.IncreaseChargeScore();
        }
    }
}
