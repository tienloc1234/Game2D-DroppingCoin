using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int coinCount = 0;
    private int score = 0;
    private int totalCoinsInMap = 0;    // mới thêm vào để đếm số coin có trong scene

    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI totalScoreText;
    [SerializeField] private TextMeshProUGUI keyText;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gameWinUI;
    //[SerializeField] private int requiredCoins = 20; đổi lại thay vì fix cứng là 20 thì sẽ lấy số coin có trong scene
    private bool isGameOver = false;
    private bool isGameWin = false;
    private bool hasKey = false;

    void Start()
    {
        totalCoinsInMap = GameObject.FindGameObjectsWithTag("Coin").Length;   // vừa vào là đếm hết tất cả coin có trong scene

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
            coinText.text = coinCount + "/" + totalCoinsInMap; // vừa sửa để hiện thị số coin đã thu thập và tổng số coin trong scene
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
        return coinCount >= totalCoinsInMap;   // vừa sửa lại để kiểm tra xem người chơi đã thu thập đủ số coin trong scene hay chưa
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
