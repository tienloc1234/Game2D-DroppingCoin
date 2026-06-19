using UnityEngine;

public class EnemyHead : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
        if (playerRb == null || playerRb.linearVelocity.y > 0) return;

        Collider2D headCollider = GetComponent<Collider2D>();
        float playerBottomY = collision.bounds.min.y;
        float headTopY = headCollider.bounds.max.y;

        if (playerBottomY < headTopY - 0.1f) return;

        Enemy enemy = GetComponentInParent<Enemy>();
        if (enemy == null || enemy.IsDead) return;

        enemy.Stomp(playerRb);
    }
}
