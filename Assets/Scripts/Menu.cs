using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}