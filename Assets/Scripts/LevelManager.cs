using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    #region SINGLETON

    //this cretes singleton instance of the Lavelmanager
    public static LevelManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    #endregion

    public float currentGameScore = 0f;

    public bool isPlaying = false;

    public UnityEvent onPlay = new UnityEvent();
    public UnityEvent onGameOver = new UnityEvent();

    [SerializeField] private TextMeshProUGUI scoreText;

    private void Update()
    {
        if (isPlaying)
        {
            currentGameScore += Time.deltaTime;
        }
    }

    public void StartGame()
    {
        onPlay.Invoke();
        isPlaying = true;
    }

    public void GameOver()
    {
        onGameOver.Invoke();
        currentGameScore = 0;
        isPlaying = false;
    }

    //Converts the timer score to a interget string
    public string VisualGameScore()
    {
        return Mathf.RoundToInt(currentGameScore).ToString();        
    }
}
