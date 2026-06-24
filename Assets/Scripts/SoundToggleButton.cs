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
        UpdateButtonImage();
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
        UpdateButtonImage();
    }

    private void UpdateButtonImage()
    {
        if (buttonImage == null)
        {
            return;
        }
        bool isMuted = AudioManager.Instance != null && AudioManager.Instance.IsMuted();
        buttonImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
    }
}