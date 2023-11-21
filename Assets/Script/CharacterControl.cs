using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState
{
    STANDING,
    RUNNING,
    JUMPING_RISE,
    JUMPING_FALL,
    FALLING,
    HANGING,
    ON_LEDGE,
    WALLJUMPING_RISE,
    WALLJUMPING_FALL,
    HURT
}

/// <summary>
/// Should store everything as a character's main class.
/// Bad practice, but for jam.
/// </summary>
public class CharacterControl : MonoBehaviour
{
    [Header("Body")]
    [SerializeField] private Transform mainTransform;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject model;

    [SerializeField] private Collider leftCollider;
    [SerializeField] private Collider rightCollider;
    [SerializeField] private Collider groundCollider;

    [Header("Stats")]

    [SerializeField] private float horizontalSpeed;

    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCountLimit;
    [SerializeField] private float lowJumpMultiplier;
    [SerializeField] private float fallSpeed;

    [SerializeField] private float wallJumpVerticalForce;
    [SerializeField] private float wallJumpHorizontalForce;

    [SerializeField] private float maxStamina;
    [SerializeField] private float maxSanity;
    [SerializeField] private float staminaRegen; // per sec
    [SerializeField] private float sanityDrain; // drain while in terrifying situations

    [SerializeField] private float staminaJumpCost;

    public float stamina;
    public float sanity;

    public bool canReduceSanity;

    [Header("State")]

    [SerializeField] private CharacterState characterState;
    [SerializeField] private float jumpCount;

    public bool grounded;
    public bool touchingRight;
    public bool touchingLeft;
    public bool wallJumping;

    [Header("Controls")]

    [SerializeField] private KeyCode moveLeft;
    [SerializeField] private KeyCode moveRight;
    [SerializeField] private KeyCode jump;
    [SerializeField] private KeyCode hang;

    [Header("Timers")]
    [SerializeField] private float wallJumpFreeDuration;


    public void Initialize()
    {

    }

    void Update()
    {
        // Detect Inputs
        if (Input.GetKey(moveLeft))
        {
            Move(true);
        }
        else if (Input.GetKey(moveRight))
        {
            Move(false);
        }
        else
        {
            // Hacky, can't be pushed by environment
            rb.velocity = new Vector3(0, rb.velocity.y);
        }

        if (Input.GetKeyDown(jump))
        {
            Jump();
        }

        if (Input.GetKey(hang))
        {
            if (!Input.GetKeyDown(jump) && stamina > staminaJumpCost)
            {
                if (touchingLeft)
                {
                    // Play anim
                    rb.constraints = RigidbodyConstraints.FreezeAll;
                }
                else if (touchingRight)
                {
                    // Play anim
                    rb.constraints = RigidbodyConstraints.FreezeAll;
                }
            }

            if (Input.GetKeyDown(jump))
            {
                WallJump();
            }
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        }

        // Jump Physics
        if (rb.velocity.y < 0)
        {
            rb.velocity += (fallSpeed - 1) * Physics.gravity.y * Time.deltaTime * Vector3.up;

            if (!grounded)
            {
                jumpCount = 0;
            }
        }
        else if (rb.velocity.y > 0 && !(Input.GetKey(KeyCode.Space)))
        {
            rb.velocity += (lowJumpMultiplier - 1) * Physics.gravity.y * Time.deltaTime * Vector3.up;
        }
    }

    private void StateAssigner(CharacterState newState)
    {
        characterState = newState;
    }

    private void Move(bool left)
    {
        List<CharacterState> validStates = new List<CharacterState> {
            CharacterState.STANDING,
            CharacterState.RUNNING,
            CharacterState.ON_LEDGE,
            CharacterState.JUMPING_FALL,
            CharacterState.FALLING
        };

        // Can't move in this situation
        if (!validStates.Contains(characterState))
        {
            return;
        }

        if (left)
        {
            rb.velocity = new Vector3(-horizontalSpeed, rb.velocity.y, 0);
        }
        else
        {
            rb.velocity = new Vector3(horizontalSpeed, rb.velocity.y, 0);
        }
    }

    private void WallJump()
    {
        if(characterState != CharacterState.HANGING)
        {
            return;
        }

        // Pay stamina first
        if (stamina > staminaJumpCost)
        {
            stamina -= staminaJumpCost;
        }
        else
        {
            return;
        }

        if (touchingLeft)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(new Vector3(wallJumpHorizontalForce, wallJumpVerticalForce, 0), ForceMode.Impulse);
        }
        else if (touchingRight)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(new Vector3(-wallJumpHorizontalForce, wallJumpVerticalForce, 0), ForceMode.Impulse);
        }
    }

    private void Jump()
    {
        List<CharacterState> validStates = new List<CharacterState> { 
            CharacterState.STANDING,
            CharacterState.RUNNING,
            CharacterState.ON_LEDGE
        };

        // Can't jump in this situation
        if (!validStates.Contains(characterState))
        {
            return;
        }

        // Pay jump count
        if(jumpCount <= 0)
        {
            return;
        }
        else
        {
            jumpCount -= 1;
        }

        // Pay stamina first
        if(stamina > staminaJumpCost)
        {
            stamina -= staminaJumpCost;
        }
        else
        {
            return;
        }

        if(characterState == CharacterState.STANDING || characterState == CharacterState.RUNNING || characterState == CharacterState.ON_LEDGE)
        {
            // Jump off ground
            rb.velocity = new Vector3(rb.velocity.x, 0);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void RefreshJump()
    {
        jumpCount = jumpCountLimit;
    }

    private void RegenStamina(float duration)
    {
        stamina += duration * staminaRegen;

        CapStats();
    }

    private void DrainSanity(float duration)
    {
        sanity -= duration * sanityDrain;

        CapStats();
    }

    private void AddSanity(int sum)
    {
        sanity += sum;

        CapStats();
    }

    private void CapStats()
    {
        if(stamina > maxStamina)
        {
            stamina = maxStamina;
        }

        if(stamina < 0)
        {
            stamina = 0;
        }

        if(sanity > maxSanity)
        {
            sanity = maxSanity;
        }

        if(sanity < 0)
        {
            sanity = 0;
        }
    }
}
