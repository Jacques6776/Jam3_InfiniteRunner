using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

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

    [Header ("Game Score Controls")]
    //Save system for scores. Creating a Serialized class (seperate script: Game Data)
    public int currentGameScore;
    public GameData gameData; // this refers to the serialized class we made
    [SerializeField] private TextMeshProUGUI scoreText;

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

    private void Start()
    {
        //if we have data then it will load
        //if not it will create new data

        //loads the save data at the start of the game
        string loadedData = SaveSystem.Load("save");
        if (loadedData != null)
        {
            //load it back into the game as readable data
            gameData = JsonUtility.FromJson<GameData>(loadedData);
        }
        else
        {
            gameData = new GameData();
        }

        playerInput = playerObject.GetComponent<PlayerInput>();

        playerController = FindFirstObjectByType<PlayerController>();
    }

    private void Update()
    {
        if (currentChargeScore == activationChargeScore)
        {
            EnablePlayerChargeAttack();
        }
    }

    public void StartGame()
    {
        onPlay.Invoke();
        isPlaying = true;

        currentGameScore = 0;

        currentChargeScore = startingChargeScore;

        playerInput.enabled = true;
    }

    public void GameOver()
    {
        isPlaying = false;

        playerInput.enabled = false;

        //want game over invoke after other code
        onGameOver.Invoke();
    }

    public void IncreaseGameScore(int points)
    {
        currentGameScore = currentGameScore + points;
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

    //Converts the timer score to a interget string
    public string VisualGameScore()
    {
        return currentGameScore.ToString();
    }

    public string VisualGameHighscore()
    {
        return Mathf.RoundToInt(gameData.highscore).ToString();
    }

    public string VisualChargeTracker()
    {
        return currentChargeScore.ToString();
    }
}
