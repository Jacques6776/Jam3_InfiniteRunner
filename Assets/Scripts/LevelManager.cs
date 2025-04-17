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

    //Save system for scores. Creating a Serialized class (seperate script: Game Data)
    public float currentGameScore = 0f;

    public GameData gameData; // this refers to the serialized class we made

    //Set to know when game is in play and when in game over states. WIll enable and disable objects as need that are listing to the singleton states
    public bool isPlaying = false;

    public UnityEvent onPlay = new UnityEvent();
    public UnityEvent onGameOver = new UnityEvent();

    [SerializeField] private TextMeshProUGUI scoreText;

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
    }

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
        currentGameScore = 0;
    }

    public void GameOver()
    {
        //compares the highscore saved in gamedata class
        if (gameData.highscore < currentGameScore)
        {
            gameData.highscore = currentGameScore;

            //this will call to the save system to update the highscore and save it
            string saveString = JsonUtility.ToJson(gameData); // converts the score into a json that can be saved by the game
            SaveSystem.Save("save", saveString);
        }
        isPlaying = false;

        //want game over invoke after other code
        onGameOver.Invoke();
    }

    //Converts the timer score to a interget string
    public string VisualGameScore()
    {
        return Mathf.RoundToInt(currentGameScore).ToString();
    }

    public string VisualGameHighscore()
    {
        return Mathf.RoundToInt(gameData.highscore).ToString();
    }
}
