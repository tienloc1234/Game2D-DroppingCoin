using UnityEngine; 

public class PlayerCollision : MonoBehaviour
{
    private GameManager gameManager;
    private AudioManager audioManager;
    private PlayerController playerController;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        audioManager = FindAnyObjectByType<AudioManager>();
        playerController = GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "FakeCoin":
                collision.GetComponent<FakeCoin>()?.TriggerTrap();
                break;

            case "Coin":
                Destroy(collision.gameObject);
                audioManager.PlayCoinSound();
                gameManager.CollectCoin();
                break;
        }
        if (collision.CompareTag("Potion"))
        {
            Destroy(collision.gameObject);
            playerController.EnableDoubleJump(12f); // 12 giây double jump
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
