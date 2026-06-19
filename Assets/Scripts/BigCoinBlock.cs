using System.Collections;
using UnityEngine;

public class BigCoinBlock : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int coinCount = 5;
    [SerializeField] private float spawnDelay = 0.12f;
    [SerializeField] private float spawnHeight = 0.5f;
    [SerializeField] private float rightOffset = 1f;
    [SerializeField] private float rightSpacing = 0.35f;
    [SerializeField] private float popRightForce = 1f;

    private bool used;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (used || coinPrefab == null) return;
        if (!collision.CompareTag("Player")) return;

        used = true;
        StartCoroutine(ReleaseCoins());

        Collider2D blockCollider = GetComponent<Collider2D>();
        if (blockCollider != null)
            blockCollider.enabled = false;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;
    }

    private IEnumerator ReleaseCoins()
    {
        for (int i = 0; i < coinCount; i++)
        {
            Vector3 spawnPos = transform.position + new Vector3(
                rightOffset + i * rightSpacing,
                spawnHeight + i * 0.15f,
                0f);

            GameObject coin = Instantiate(coinPrefab, spawnPos, Quaternion.identity);

            DroppedCoin droppedCoin = coin.AddComponent<DroppedCoin>();
            droppedCoin.Launch(new Vector2(popRightForce, 0f));

            yield return new WaitForSeconds(spawnDelay);
        }

        Destroy(gameObject, 1f);
    }
}
