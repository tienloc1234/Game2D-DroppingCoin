using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private GameManager gameManager;
    private AudioManager audioManager;
    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            audioManager.PlayCoinSound();
            gameManager.AddScore(1);
        }

        if (collision.CompareTag("Trap"))
        {
            gameManager.GameOver();
        }

        if (collision.CompareTag("DeathZone"))
        {
            gameManager.GameOver();
        }

        if (collision.CompareTag("Enemy"))
        {
            gameManager.GameOver();
        }

        if (collision.CompareTag("Key"))
        {
            Destroy(collision.gameObject);
            //gameManager.CollectKey();
            gameManager.GameWin();
        }
    }
}