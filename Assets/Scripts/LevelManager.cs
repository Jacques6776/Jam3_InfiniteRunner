using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{    
    [Header("Game Menus")]
    [SerializeField] private GameObject startMenuUI;
    [SerializeField] private GameObject gameOverUI;

    [Header("Game Scores")]
    [SerializeField] private TextMeshProUGUI startScreenScore;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameOverScore;
    [SerializeField] private TextMeshProUGUI gameOverHighscore;

    [Header("Charge Tracker")]
    //[SerializeField] private TextMeshProUGUI chargeIndicatorText;
    [SerializeField] private Slider chargeSlider;

    [Header ("Game Score Controls")]
    public int currentGameScore;
    //public GameData gameData; // this refers to the serialized class we made
    public int gameOverHighScore;

    [Header ("Game State Controls")]
    //Set to know when game is in play and when in game over states. WIll enable and disable objects as need that are listing to the singleton states
    public bool isPlaying = false;
    public UnityEvent onPlay = new UnityEvent();
    public UnityEvent onGameOver = new UnityEvent();

    [Header("Charge Attack Tracker")]
    [SerializeField] private int startingChargeScore = 0;
    [SerializeField] private int activationChargeScore = 10;
    [SerializeField] private int currentChargeScore;    

    public GameObject playerObject;
    private PlayerInput playerInput;
    public PlayerController playerController;

    public ObstacleSpawnController obstacleSpawnController;

    private void Start()
    {
        playerInput = playerObject.GetComponent<PlayerInput>();

        playerController = FindFirstObjectByType<PlayerController>();

        obstacleSpawnController = FindFirstObjectByType<ObstacleSpawnController>();

        chargeSlider.maxValue = activationChargeScore;
        chargeSlider.value = currentChargeScore;
    }

    private void Update()
    {
        if (currentChargeScore == activationChargeScore)
        {
            EnablePlayerChargeAttack();
        }

        ScoreTextDisplay();

        startScreenScore.text = "Highscore: " + PlayerPrefs.GetInt("SavedHighScore").ToString();

        chargeSlider.value = currentChargeScore;
    }

    //game states
    public void StartGame()
    {        
        if(playerObject.activeInHierarchy == false)
        {
            playerObject.SetActive(true);
        }
        
        isPlaying = true;

        currentGameScore = 0;

        currentChargeScore = startingChargeScore;

        playerInput.enabled = true;

        obstacleSpawnController.ActivateSpawner(isPlaying);
    }

    public void GameOver()
    {
        isPlaying = false;

        playerInput.enabled = false;

        obstacleSpawnController.DeactivateSpawner(isPlaying);

        gameOverUI.SetActive(true);

        HighScoreUpdate();

        gameOverScore.text = "Score: " + currentGameScore.ToString();
        gameOverHighscore.text = "Highscore: " + PlayerPrefs.GetInt("SavedHighScore").ToString();
    }
    
    //active game score tracking
    public void IncreaseGameScore(int points)
    {
        currentGameScore = currentGameScore + points;
    }

    private void ScoreTextDisplay()
    {
        scoreText.text = currentGameScore.ToString();
    }

    //game high score
    public void HighScoreUpdate()
    {
        //check if there is a high score
        if(PlayerPrefs.HasKey("SavedHighScore"))
        {
            //if score is higher
            if(currentGameScore > PlayerPrefs.GetInt("SavedHighScore"))
            {
                //Set the new high score
                PlayerPrefs.SetInt("SavedHighScore", currentGameScore);
            }
        }
        else
        {
            //if no score yet set
            PlayerPrefs.SetInt("SavedHighScore", currentGameScore); ;
        }
    }

    //Charge score controlls
    public void IncreaseChargeScore()
    {
        if (currentChargeScore < activationChargeScore)
        {
            currentChargeScore++;
        }
        else
        {
            return;
        }
    }

    public void ResetChargeScore()
    {
        currentChargeScore = startingChargeScore;
    }

    private bool EnablePlayerChargeAttack()
    {
        return playerController.canCharge = true;
    }
}
