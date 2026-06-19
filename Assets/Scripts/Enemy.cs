using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speedenemy = 2f;
    [SerializeField] private float distance = 5f;
    [SerializeField] private float stompBounceForce = 10f;

    private Vector3 startPos;
    private bool movingRight = true;
    private bool isDead = false;

    public bool IsDead => isDead;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (isDead) return;

        float leftBound = startPos.x - distance;
        float rightBound = startPos.x + distance;

        if (movingRight)
        {
            transform.Translate(Vector2.right * speedenemy * Time.deltaTime);
            if (transform.position.x >= rightBound)
            {
                movingRight = false;
                Flip();
            }
        }
        else
        {
            transform.Translate(Vector2.left * speedenemy * Time.deltaTime);
            if (transform.position.x <= leftBound)
            {
                movingRight = true;
                Flip();
            }
        }
    }

    public void Stomp(Rigidbody2D playerRb)
    {
        if (isDead) return;

        isDead = true;

        foreach (Collider2D collider in GetComponentsInChildren<Collider2D>())
            collider.enabled = false;

        if (playerRb != null)
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, stompBounceForce);

        Destroy(gameObject, 0.1f);
    }

    void Flip()
    {
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}
