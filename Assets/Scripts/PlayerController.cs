using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float doubleJumpForce = 12f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;

    // Dùng để leo cầu thang ở map rừng rậm
    [Header("Ladder")]
    [SerializeField] private float climbSpeed = 4f;
    [SerializeField] private string ladderTag = "Ladder";
    private bool isOnLadder = false;
    private bool isTouchingLadder = false;
    private float originalGravityScale;

    //Mới được thêm vào để sa lầy ở map 3
    [Header("Sand")]
    [SerializeField] private float sandSpeedMultiplier = 0.4f;
    [SerializeField] private float sandAcceleration = 15f;
    [SerializeField] private string sandTag = "Sand";

    // Moi them vao de truot bang
    [Header("Movement Feel")]
    [SerializeField] private float groundAcceleration = 60f;
    [SerializeField] private float groundDeceleration = 60f;
    [SerializeField] private float iceAcceleration = 20f;
    [SerializeField] private float iceDeceleration = 4f;
    [SerializeField] private string iceTag = "Ice";


    private Animator animator;
    private bool isGrounded;
    private bool isOnIce; // moi them vao de truot bang
    private bool isOnSand; // Mới thêm vào để sa lầy map 3
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
        originalGravityScale = rb.gravityScale;

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
        HandleLadder();
        UpdateAnimation();
    }
    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");

        float speedMultiplier = isOnSand ? sandSpeedMultiplier : 1f;
        float targetSpeed = moveInput * moveSpeed * speedMultiplier;

        float accel = isOnIce ? iceAcceleration : (isOnSand ? sandAcceleration : groundAcceleration);
        float decel = isOnIce ? iceDeceleration : (isOnSand ? sandAcceleration : groundDeceleration);

        float rate = (Mathf.Abs(targetSpeed) > 0.01f) ? accel : decel;

        float newX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, rate * Time.deltaTime);
        rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);

        if (moveInput > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    private bool wasGrounded;
    

    private void HandleJump()
    {
        Collider2D groundHit = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        //them moi vao de truot bang
        isGrounded = groundHit != null;
        isOnIce = isGrounded && groundHit.CompareTag(iceTag);
        isOnSand = isGrounded && groundHit.CompareTag(sandTag);


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

    // Hàm này dùng để xử lý việc leo cầu thang trong game. Khi người chơi chạm vào cầu thang và nhấn nút di chuyển lên hoặc xuống, 
    //nhân vật sẽ bắt đầu leo cầu thang. Trong khi leo, trọng lực của nhân vật sẽ bị vô hiệu hóa để cho phép di chuyển theo chiều dọc. 
    //Nếu người chơi nhấn nút nhảy trong khi đang leo cầu thang, nhân vật sẽ thoát khỏi trạng thái leo và rơi xuống với lực nhảy được áp dụng.
    private void HandleLadder()
    {
        float vertical = Input.GetAxis("Vertical");

        if (isTouchingLadder && Mathf.Abs(vertical) > 0.1f)
        {
            isOnLadder = true;
            rb.gravityScale = 0f;
        }

        if (isOnLadder)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, vertical * climbSpeed);

            if (Input.GetButtonDown("Jump"))
            {
                isOnLadder = false;
                rb.gravityScale = originalGravityScale;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(ladderTag))
        {
            isTouchingLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(ladderTag))
        {
            isTouchingLadder = false;
            isOnLadder = false;
            rb.gravityScale = originalGravityScale;
        }
    }
}
