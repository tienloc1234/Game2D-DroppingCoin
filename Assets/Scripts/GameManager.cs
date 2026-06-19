using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int coinCount = 0;
    private int score = 0;

    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI totalScoreText;
    [SerializeField] private TextMeshProUGUI keyText;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gameWinUI;
    [SerializeField] private int requiredCoins = 20;
    private bool isGameOver = false;
    private bool isGameWin = false;
    private bool hasKey = false;

    void Start()
    {
        UpdateUI();
        gameOverUI.SetActive(false);
        gameWinUI.SetActive(false);
        keyText.text = "0";
    }

    public void CollectCoin()
    {
        if (isGameOver || isGameWin) return;

        coinCount++;
        score++;
        UpdateCoinUI();
        UpdateScoreUI();
    }

    private void UpdateCoinUI()
    {
        if (coinText != null)
            coinText.text = coinCount.ToString();
    }

    private void UpdateScoreUI()
    {
        if (totalScoreText != null)
            totalScoreText.text = score.ToString();
    }

    private void UpdateUI()
    {
        UpdateCoinUI();
        UpdateScoreUI();
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
        coinCount = 0;
        score = 0;
        UpdateUI();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }

    public void CollectKey()
    {
        hasKey = true;
        keyText.text = "1";
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

    public bool HasAllCoins()
    {
        return coinCount >= requiredCoins;
    }

    public void NextLevel()
    {
        Time.timeScale = 1;
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
