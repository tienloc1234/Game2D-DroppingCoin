using System.Collections;
using UnityEngine;

public class BigCoinBlock : MonoBehaviour
{
    private enum ReleaseLayout
    {
        Horizontal,
        Vertical
    }

    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int coinCount = 5;
    [SerializeField] private float spawnDelay = 0.08f;
    [SerializeField] private float coinSpacing = 0.5f;
    [SerializeField] private float lineOffset = 0.5f;

    private static readonly float CoinCheckRadius = 0.15f;
    private LayerMask groundLayer;

    private bool used;

    void Awake()
    {
        groundLayer = LayerMask.GetMask("Ground");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (used || coinPrefab == null) return;
        if (!collision.CompareTag("Player")) return;

        used = true;
        StartCoroutine(ReleaseCoins(collision));

        Collider2D blockCollider = GetComponent<Collider2D>();
        if (blockCollider != null)
            blockCollider.enabled = false;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;
    }

    private IEnumerator ReleaseCoins(Collider2D player)
    {
        //Collider2D blockCollider = GetComponent<Collider2D>();
        //Bounds bounds = blockCollider != null
        //    ? blockCollider.bounds
        //    : new Bounds(transform.position, Vector3.one * 2f);
        Collider2D blockCollider = GetComponent<Collider2D>();
        Bounds bounds = blockCollider.bounds;

        Bounds playerBounds = player.bounds;
        ReleaseLayout layout = GetReleaseLayout(playerBounds, bounds);
        bool playerOnLeft = playerBounds.center.x < bounds.center.x;

        for (int i = 0; i < coinCount; i++)
        {
            Vector3 spawnPos = GetClearSpawnPosition(
                GetCoinPosition(bounds, layout, playerOnLeft, i));

            GameObject coin = Instantiate(coinPrefab, spawnPos, Quaternion.identity);
            coin.AddComponent<DroppedCoin>();

            yield return new WaitForSeconds(spawnDelay);
        }

        Destroy(gameObject, 0.5f);
    }

    private ReleaseLayout GetReleaseLayout(Bounds playerBounds, Bounds blockBounds)
    {
        bool fromTop = playerBounds.min.y >= blockBounds.max.y - 0.3f;
        bool fromBottom = playerBounds.max.y <= blockBounds.min.y + 0.3f;

        if (fromTop || fromBottom)
            return ReleaseLayout.Vertical;

        return ReleaseLayout.Horizontal;
    }

    private Vector3 GetCoinPosition(Bounds bounds, ReleaseLayout layout, bool playerOnLeft, int index)
    {
        float spawnY = bounds.max.y + lineOffset;

        if (layout == ReleaseLayout.Horizontal)
        {
            if (playerOnLeft)
            {
                float startX = bounds.max.x + lineOffset;
                return new Vector3(startX + index * coinSpacing, spawnY, 0f);
            }

            float startXLeft = bounds.min.x - lineOffset;
            return new Vector3(startXLeft - index * coinSpacing, spawnY, 0f);
        }

        float topY = spawnY + (coinCount - 1) * coinSpacing;
        return new Vector3(bounds.center.x, topY - index * coinSpacing, 0f);
    }

    private Vector3 GetClearSpawnPosition(Vector3 desired)
    {
        if (groundLayer == 0) return desired;

        if (Physics2D.OverlapCircle(desired, CoinCheckRadius, groundLayer) == null)
            return desired;

        for (int i = 1; i <= 10; i++)
        {
            Vector3 higher = desired + Vector3.up * (i * 0.4f);
            if (Physics2D.OverlapCircle(higher, CoinCheckRadius, groundLayer) == null)
                return higher;
        }

        return desired + Vector3.up * 4f;
    }
}
