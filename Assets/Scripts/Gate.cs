using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (gameManager.HasKey() && gameManager.HasAllCoins())
            {
                gameManager.GameWin();
            }
        }
    }
}
