using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float doubleJumpForce = 12f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    private Animator animator;
    private bool isGrounded;
    private Rigidbody2D rb;
    private GameManager gameManager;
    private AudioManager audioManager;

    private int jumpCount;
    private int maxJumpCount = 1;
    private Coroutine doubleJumpTimer;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gameManager = FindAnyObjectByType<GameManager>();
        
       //audioManager = FindAnyObjectByType<AudioManager>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.IsGameOver() || gameManager.IsGameWin()) return;
        HandleMovement();
        HandleJump();
        UpdateAnimation();
    }
    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        if (moveInput > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    private bool wasGrounded;
    

    private void HandleJump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (isGrounded && !wasGrounded)
        {
            jumpCount = 0;
        }

        bool canJump = jumpCount < maxJumpCount;

        if (Input.GetButtonDown("Jump") && canJump)
        {
            float forceToUse = isGrounded ? jumpForce : doubleJumpForce;
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayJumpSound();
            }
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forceToUse);
            jumpCount++;
        }

        wasGrounded = isGrounded;
    }

    private void UpdateAnimation()
    {
        bool IsRunning = Mathf.Abs(rb.linearVelocity.x) > 0.1f;
        bool IsJumping = !isGrounded;
        animator.SetBool("IsRunning", IsRunning);
        animator.SetBool("IsJumping", IsJumping);
    }
    public void EnableDoubleJump(float duration)
    {
        if (doubleJumpTimer != null)
        {
            StopCoroutine(doubleJumpTimer);
        }
        doubleJumpTimer = StartCoroutine(DoubleJumpCountdown(duration));
    }

    private System.Collections.IEnumerator DoubleJumpCountdown(float duration)
    {
        maxJumpCount = 2;

        yield return new WaitForSeconds(duration);

        maxJumpCount = 1;
        doubleJumpTimer = null;
    }
}
