using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private GameManager gameManager;
    private AudioManager audioManager;
    private Rigidbody2D rb;
    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        audioManager = FindAnyObjectByType<AudioManager>();
        rb = GetComponent<Rigidbody2D>();
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
           float enemyTopY = collision.bounds.max.y;
            float playerBottomY = GetComponent<Collider2D>().bounds.min.y;
            
            if (playerBottomY > enemyTopY - 0.25f && rb.linearVelocity.y <= 0)
            {
                KillEnemy(collision.gameObject);
            }
            else
            {
                gameManager.GameOver();
            }
        }

        if (collision.CompareTag("Key"))
        {
            Destroy(collision.gameObject);
            gameManager.CollectKey();
            //gameManager.GameWin();
        }
    }
    private void KillEnemy(GameObject enemy)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 10f);
        gameManager.AddScore(1);
        Destroy(enemy);
    }
}