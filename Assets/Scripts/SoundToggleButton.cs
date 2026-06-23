using UnityEngine;
using UnityEngine.UI;

public class SoundToggleButton : MonoBehaviour
{
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    private Image buttonImage;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
    }

    private void Start()
    {
        ApplySound();
        UpdateButtonImage();
    }

    public void ToggleSound()
    {
        bool isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;

        isMuted = !isMuted;

        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
        PlayerPrefs.Save();

        ApplySound();
        UpdateButtonImage();

        Debug.Log(isMuted ? "Sound OFF" : "Sound ON");
    }

    private void ApplySound()
    {
        bool isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;
        AudioListener.volume = isMuted ? 0f : 1f;
    }

    private void UpdateButtonImage()
    {
        bool isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;

        if (buttonImage == null)
        {
            Debug.LogWarning("Không tìm th?y Image trên nút Sound");
            return;
        }

        buttonImage.sprite = isMuted ? soundOffSprite : soundOnSprite;

        Debug.Log(isMuted ? "?ã ??i ?nh sang SOUND OFF" : "?ã ??i ?nh sang SOUND ON");
    }
}