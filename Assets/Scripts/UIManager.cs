using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{    
    [SerializeField] private GameObject startMenuUI;
    [SerializeField] private GameObject gameOverUI;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameOverScore;
    [SerializeField] private TextMeshProUGUI gameOverHighscore;

    LevelManager levelManager;

    private void Start()
    {
        levelManager = LevelManager.Instance;
        levelManager.onGameOver.AddListener(ActivateGameOverUI);
    }

    private void Update()
    {
        ScoreTextDisplay();
    }

    public void PlayButtonHandler()
    {
        levelManager.StartGame();
    }

    public void ActivateGameOverUI()
    {
        gameOverUI.SetActive(true);

        gameOverScore.text = "Score: " + levelManager.VisualGameScore();
        gameOverHighscore.text = "Highscore: " + levelManager.VisualGameHighscore();
    }

    private void ScoreTextDisplay()
    {
        scoreText.text = levelManager.VisualGameScore();
    }
}
