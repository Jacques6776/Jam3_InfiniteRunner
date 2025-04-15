using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Some code referenced from Muddy Wolf https://www.youtube.com/watch?v=lVtL_xD-gTg&list=PLfX6C2dxVyLylMufxTi7DM9Vjlw5bff1c&index=2

    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private Transform GFX;

    [Header("Jump Controls")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayer;    
    [SerializeField] private Transform groundChecker;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private float jumpTime = 0.3f;
    private float jumpTimer;

    [Header("Crouch Controls")]
    [SerializeField] private float crouchHeight = 0.5f;

    public bool isGrounded = false;
    public bool isJumping = false;

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundChecker.position, groundCheckRadius, groundLayer);

        #region JUMPING

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            playerRB.linearVelocity = Vector2.up * jumpForce;
        }

        if (isJumping && Input.GetButton("Jump"))
        { 
            if (jumpTimer < jumpTime)
            {
                playerRB.linearVelocity = Vector2.up * jumpForce;

                jumpTimer += Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetButtonUp ("Jump"))
        {
            isJumping = false;
            jumpTimer = 0f;
        }

        #endregion

        #region CROUCHING

        if (isGrounded && Input.GetButton ("Crouch"))
        {
            GFX.localScale = new Vector3(GFX.localScale.x, crouchHeight, GFX.localScale.z);

            if (isJumping)
            {
                GFX.localScale = new Vector3(GFX.localScale.x, 1f, GFX.localScale.z); //Want to strade out the one for a saved initial scale
            }
        }
        if (Input.GetButtonUp ("Crouch"))
        {
            GFX.localScale = new Vector3(GFX.localScale.x, 1f, GFX.localScale.z); //Want to strade out the one for a saved initial scale
        }

        #endregion        

    }

    #region COLLISIONS

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Obstacle")
        {
            Destroy(gameObject);

            LevelManager.Instance.GameOver();
        }
    }

    #endregion
}
