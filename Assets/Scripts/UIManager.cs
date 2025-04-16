using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject startMenuUI;
    [SerializeField] private GameObject gameOverUI;

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
    }

    private void ScoreTextDisplay()
    {
        scoreText.text = levelManager.VisualGameScore();
    }
}
