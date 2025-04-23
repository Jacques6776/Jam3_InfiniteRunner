using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Some code referenced from Muddy Wolf https://www.youtube.com/watch?v=lVtL_xD-gTg&list=PLfX6C2dxVyLylMufxTi7DM9Vjlw5bff1c&index=2

    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private Transform playerObject;

    public bool isGrounded = false;

    [Header("Jump Controls")]
    [SerializeField] private float jumpForce = 10f;
    public bool isJumping = false;
    public bool doubleJump = false;

    [Header("Crouch Controls")]
    [SerializeField] private float crouchHeight = 0.5f;
    public bool isCrawling = false;

    [Header("Charge Controls")]
    public bool canCharge = false;
    [SerializeField] bool isCharging = false;
    [SerializeField] bool resetCharge = false;

    public Transform chargeEndPosition;
    public Transform resetChargePosition;
        
    [SerializeField] private float currentChargeSpeed;
    [SerializeField] private float forwardChargeSpeed = 5f;
    [SerializeField] private float retreatChargeSpeed = 3f;


    //Input actions
    private PlayerInputMap input = null;
    [SerializeField] PlayerInputMap playerInputs;

    private LevelManager levelManager;

    private void Awake()
    {
        input = new PlayerInputMap();

        levelManager = FindFirstObjectByType<LevelManager>();
    }

    private void Start()
    {
        
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
        if (isGrounded)
        {
            isJumping = false;
            doubleJump = false;
        }
        else
        {
            isJumping = true;
        }

        if (isCrawling)
        {
            if (isJumping)
            {
                return;
            }
            else
            {
                PlayerIsCrawling();
            }
        }
        else
        {
            PlayerIsNotCrawling();
        }

        if(isCharging) // needs to know it should now prioritise point 2
        {
            ChargeAttack();
        }

        if(resetCharge)
        {
            ResetChargeAttack();
        }
    }

    private void ActivatePlayer()
    {
        gameObject.SetActive (true);
    }

    //jump controls
    public void PlayerJumpingSwitch(InputAction.CallbackContext context)
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

    public void PlayerCrawlSwitch(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            //Debug.Log("Is crawling");         
        }
    }

    private void PlayerIsCrawling()
    {
        if (isJumping)
        {
            playerObject.localScale = new Vector3(playerObject.localScale.x, 1f, playerObject.localScale.z); //Scales with be set to animations
        }
            playerObject.localScale = new Vector3(playerObject.localScale.x, crouchHeight, playerObject.localScale.z);
    }

    private void PlayerIsNotCrawling()
    {
        playerObject.localScale = new Vector3(playerObject.localScale.x, 1f, playerObject.localScale.z);
    }

    public void PlayerChargeAttackSwitch(InputAction.CallbackContext context)
    {
        if (canCharge && context.performed)
        {            
            isCharging = true;
        }
    }

    private void ChargeAttack()
    {
        canCharge = false;
        gameObject.tag = "ChargeState";
        levelManager.ResetChargeScore();
        currentChargeSpeed = forwardChargeSpeed;
        transform.position = Vector2.MoveTowards(transform.position, chargeEndPosition.position, Time.deltaTime * currentChargeSpeed);

        if (transform.position == chargeEndPosition.position)
        {
            isCharging = false;
            resetCharge = true;
        }
    }

    private void ResetChargeAttack()
    {
        currentChargeSpeed = retreatChargeSpeed;
        transform.position = Vector2.MoveTowards(transform.position, resetChargePosition.position, Time.deltaTime * currentChargeSpeed);

        if (transform.position == resetChargePosition.position)
        {
            resetCharge = false;
            gameObject.tag = "Player";
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Obstacle" && !isCharging)
        {
            gameObject.SetActive(false);

            levelManager.GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
