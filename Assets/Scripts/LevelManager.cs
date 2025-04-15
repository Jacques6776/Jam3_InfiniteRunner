using TMPro;
using UnityEngine;

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

    [SerializeField] private TextMeshProUGUI scoreText;

    private void Update()
    {
        if (isPlaying)
        {
            currentGameScore += Time.deltaTime;
        }

        if (Input.GetKeyDown("r"))
        {
            isPlaying = true;
        }
    }

    public void GameOver()
    {
        currentGameScore = 0;
        isPlaying = false;
    }

    //Converts the timer score to a interget string
    public string VisualGameScore()
    {
        return Mathf.RoundToInt(currentGameScore).ToString();        
    }
}
