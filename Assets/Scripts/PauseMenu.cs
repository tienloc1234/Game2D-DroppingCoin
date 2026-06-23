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
    private bool isSoundOn = true;

    void Start()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }

        Time.timeScale = 1f;

        if (PlayerPrefs.HasKey("SoundOn"))
        {
            isSoundOn = PlayerPrefs.GetInt("SoundOn") == 1;
        }

        ApplySoundState();
        UpdateSoundImage();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }

        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }

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
        isSoundOn = !isSoundOn;

        PlayerPrefs.SetInt("SoundOn", isSoundOn ? 1 : 0);
        PlayerPrefs.Save();

        ApplySoundState();
        UpdateSoundImage();
    }

    void ApplySoundState()
    {
        AudioListener.volume = isSoundOn ? 1f : 0f;
    }

    void UpdateSoundImage()
    {
        if (soundButtonImage != null)
        {
            soundButtonImage.sprite = isSoundOn ? soundOnSprite : soundOffSprite;
        }
    }
}