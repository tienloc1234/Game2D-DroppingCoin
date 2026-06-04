using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI keyText;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gameWinUI;
    private bool isGameOver = false;
    private bool isGameWin = false;
    private bool hasKey = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateScore();
        gameOverUI.SetActive(false);
        gameWinUI.SetActive(false);
        keyText.text = "Keys: 0";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddScore(int amount)
    {
        if (!isGameOver && !isGameWin)
        {
            score += amount;
            UpdateScore();
        }
    }

    private void UpdateScore()
    {
        scoreText.text = score.ToString();
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;
        gameOverUI.SetActive(true);
    }
    public void GameWin()
    {
        isGameWin = true;
        Time.timeScale = 0;
        gameWinUI.SetActive(true);
    }
    public void RestartGame()
    {
        isGameOver = false;
        score = 0;
        UpdateScore();
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }
    public void CollectKey()
    {
        hasKey = true;
        keyText.text = "Key: 1";
    }

    public bool HasKey()
    {
        return hasKey;
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }
    public bool IsGameWin()
    {
        return isGameWin;
    }
}
