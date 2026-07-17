using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Image soundButtonImage;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;
    private bool isPaused = false;

    void Start()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        Time.timeScale = 1f;
        UpdateSoundImage();
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.OnSoundStateChanged += OnSoundChanged;
        }
    }

    private void OnDestroy()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.OnSoundStateChanged -= OnSoundChanged;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void ToggleSound()
    {
        if (AudioManager.Instance == null)
        {
            return;
        }
        AudioManager.Instance.ToggleSound();
    }

    private void OnSoundChanged(bool isMuted)
    {
        UpdateSoundImage();
    }

    private void UpdateSoundImage()
    {
        if (soundButtonImage != null)
        {
            bool isMuted = AudioManager.Instance != null && AudioManager.Instance.IsMuted();
            soundButtonImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
        }
    }
}