using UnityEngine;

public class DroppedCoin : MonoBehaviour
{
    private Vector2 velocity;
    private bool landed;
    private LayerMask groundLayer;

    private const float GroundCheckRadius = 0.12f;
    private const float GroundCheckDistance = 0.35f;

    void Awake()
    {
        groundLayer = LayerMask.GetMask("Ground");
        ResolveSpawnOverlap();
    }

    private void ResolveSpawnOverlap()
    {
        if (groundLayer == 0) return;

        for (int i = 0; i < 12; i++)
        {
            if (Physics2D.OverlapCircle(transform.position, GroundCheckRadius, groundLayer) == null)
                return;

            transform.position += Vector3.up * 0.25f;
        }
    }

    void Update()
    {
        if (landed) return;

        velocity += Physics2D.gravity * 3f * Time.deltaTime;
        Vector2 move = velocity * Time.deltaTime;
        Vector2 nextPos = (Vector2)transform.position + move;

        if (groundLayer != 0 && move.y <= 0f)
        {
            RaycastHit2D hit = Physics2D.CircleCast(
                transform.position,
                GroundCheckRadius,
                Vector2.down,
                Mathf.Max(GroundCheckDistance, Mathf.Abs(move.y) + 0.05f),
                groundLayer);

            if (hit.collider != null)
            {
                transform.position = new Vector3(
                    nextPos.x,
                    hit.point.y + GroundCheckRadius + 0.05f,
                    transform.position.z);
                landed = true;
                Destroy(this);
                return;
            }
        }

        transform.position = nextPos;
    }
}
