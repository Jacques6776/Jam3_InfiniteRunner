using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Some code referenced from Muddy Wolf https://www.youtube.com/watch?v=lVtL_xD-gTg&list=PLfX6C2dxVyLylMufxTi7DM9Vjlw5bff1c&index=2

    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private Transform GFX;

    public bool isGrounded = false;

    [Header("Jump Controls")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayer;    
    [SerializeField] private Transform groundChecker;
    [SerializeField] private float groundCheckRadius = 0.2f;
    //[SerializeField] private float jumpTime = 0.3f;
    //private float jumpTimer;
    public bool isJumping = false;
    public bool doubleJump = false;

    [Header("Crouch Controls")]
    [SerializeField] private float crouchHeight = 0.5f;
    public bool isCrawling = false;

    //Input actions
    private PlayerInputMap input = null;
    [SerializeField] PlayerInputMap playerInputs;

    private void Awake()
    {
        input = new PlayerInputMap();
    }

    private void Start()
    {
        LevelManager.Instance.onPlay.AddListener(ActivatePlayer);
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Crawl.performed += PlayerCrawlEnabled;
        input.Player.Crawl.canceled += PlayerCrawlDisabled;
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void FixedUpdate()
    {
        isGrounded = GroundChecker();

        //if (isGrounded)
        //{
        //    isJumping = false;
        //    doubleJump = false;
        //}
        
        //if (isGrounded && Input.GetButton ("Crouch"))
        //{
        //    GFX.localScale = new Vector3(GFX.localScale.x, crouchHeight, GFX.localScale.z); //shrinks the sprite, will remove once animation is in

        //    if (isJumping)
        //    {
        //        GFX.localScale = new Vector3(GFX.localScale.x, 1f, GFX.localScale.z); //Want to strade out the one for a saved initial scale
        //    }
        //}
        //if (Input.GetButtonUp ("Crouch"))
        //{
        //    GFX.localScale = new Vector3(GFX.localScale.x, 1f, GFX.localScale.z); //Want to strade out the one for a saved initial scale
        //}
    }

    private void ActivatePlayer()
    {
        gameObject.SetActive (true);
    }

    private bool GroundChecker()
    {
        if (playerRB.linearVelocity.y <= 0)
        {
            Collider2D collider = Physics2D.OverlapCircle(groundChecker.position, groundCheckRadius, groundLayer);
            if (collider != gameObject)
            {
                return true;
            }            
        }
        return false;
    }

    //jump controls
    public void PlayerJumping(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isGrounded)
            {
                isJumping = true;
                playerRB.linearVelocity = Vector2.up * jumpForce;
            }
            else if (isJumping && !doubleJump)
            {
                doubleJump = true;
                playerRB.linearVelocity = Vector2.up * jumpForce;
            }            
        }
    }

    //crawl controls
    private void PlayerCrawlEnabled(InputAction.CallbackContext context)
    {
        isCrawling = true;
    }

    private void PlayerCrawlDisabled(InputAction.CallbackContext context)
    {
        isCrawling = false;
    }

    public void PlayerCrawl(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            if (isCrawling)
            {
                GFX.localScale = new Vector3(GFX.localScale.x, crouchHeight, GFX.localScale.z); //shrinks the sprite, will remove once animation is in

                if (isJumping)
                {
                    GFX.localScale = new Vector3(GFX.localScale.x, 1f, GFX.localScale.z); //Scales with be set to animations
                }
            }
            //else 
            //{
            //    GFX.localScale = new Vector3(GFX.localScale.x, 1f, GFX.localScale.z);
            //}            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Obstacle")
        {
            gameObject.SetActive(false);

            LevelManager.Instance.GameOver();
        }
    }
}
