using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource backgroundAudioSource;
    [SerializeField] private AudioSource effectAudioSource;
    [SerializeField] private AudioClip backgroundClip;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip coinClip;

    private const string MUTED_KEY = "Muted";
    private bool isMuted = false;
    public delegate void SoundStateChanged(bool isMuted);
    public event SoundStateChanged OnSoundStateChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        isMuted = PlayerPrefs.GetInt(MUTED_KEY, 0) == 1;
        ApplyMuteState();
    }

    void Start()
    {
        PlayBackgroundMusic();
    }

    private void PlayBackgroundMusic()
    {
        backgroundAudioSource.clip = backgroundClip;
        backgroundAudioSource.Play();
    }

    public void PlayCoinSound()
    {
        effectAudioSource.PlayOneShot(coinClip);
    }

    //public void PlayJumpSound()
    //{
    //    effectAudioSource.PlayOneShot(jumpClip);
    //}
    public void PlayJumpSound()
    {
        Debug.Log($"effectAudioSource = {effectAudioSource}");

        if (effectAudioSource == null)
        {
            Debug.LogError("EffectAudioSource đã bị mất!");
            return;
        }

        effectAudioSource.PlayOneShot(jumpClip);
    }

    public bool IsMuted()
    {
        return isMuted;
    }

    public void ToggleSound()
    {
        isMuted = !isMuted;
        PlayerPrefs.SetInt(MUTED_KEY, isMuted ? 1 : 0);
        PlayerPrefs.Save();
        ApplyMuteState();
        OnSoundStateChanged?.Invoke(isMuted);
    }


    private void OnDestroy()
    {
        Debug.Log("AudioManager bị Destroy");
    }

    private void ApplyMuteState()
    {
        AudioListener.volume = isMuted ? 0f : 1f;
    }
}