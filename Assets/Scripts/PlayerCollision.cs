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
            FakeCoin fakeCoin = collision.GetComponent<FakeCoin>();
            if (fakeCoin != null)
            {
                fakeCoin.TriggerTrap();
                return;
            }

            Destroy(collision.gameObject);
            audioManager.PlayCoinSound();
            gameManager.CollectCoin();
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
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null && enemy.IsDead) return;

            gameManager.GameOver();
        }

        if (collision.CompareTag("Key"))
        {
            Destroy(collision.gameObject);
            gameManager.CollectKey();
        }
    }
}
