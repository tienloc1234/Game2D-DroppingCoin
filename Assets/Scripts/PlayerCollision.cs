using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
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
            gameManager.CollectKey();
            //gameManager.GameWin();
        }
    }
}
