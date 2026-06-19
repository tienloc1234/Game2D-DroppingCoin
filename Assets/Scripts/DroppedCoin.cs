using UnityEngine;

public class DroppedCoin : MonoBehaviour
{
    private Vector2 velocity;
    private bool landed;
    private LayerMask groundLayer;

    void Awake()
    {
        groundLayer = LayerMask.GetMask("Ground");
    }

    public void Launch(Vector2 initialVelocity)
    {
        velocity = initialVelocity;
    }

    void Update()
    {
        if (landed) return;

        velocity += Physics2D.gravity * 2f * Time.deltaTime;
        Vector2 move = velocity * Time.deltaTime;

        if (groundLayer != 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                move.normalized,
                move.magnitude + 0.05f,
                groundLayer);

            if (hit.collider != null)
            {
                transform.position = hit.point + Vector2.up * 0.15f;
                landed = true;
                Destroy(this);
                return;
            }
        }

        transform.position += (Vector3)move;
    }
}
