using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    LevelManager levelManager;

    private void Start()
    {
        levelManager = LevelManager.Instance;
    }

    private void Update()
    {
        ScoreTextDisplay();
    }

    private void ScoreTextDisplay()
    {
        scoreText.text = levelManager.VisualGameScore();
    }
}
