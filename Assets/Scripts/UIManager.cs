using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{    
    [SerializeField] private GameObject startMenuUI;
    [SerializeField] private GameObject gameOverUI;

    [Header("Game Scores")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameOverScore;
    [SerializeField] private TextMeshProUGUI gameOverHighscore;

    [Header("Charge Tracker")]
    [SerializeField] private TextMeshProUGUI chargeIndicatorText;

    LevelManager levelManager;

    private void Start()
    {
        levelManager = LevelManager.Instance;
        levelManager.onGameOver.AddListener(ActivateGameOverUI);
    }

    private void Update()
    {
        ScoreTextDisplay();
        ChargeVisualIndicator();
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

    private void ChargeVisualIndicator()
    {
        chargeIndicatorText.text = levelManager.VisualChargeTracker();
    }
}
