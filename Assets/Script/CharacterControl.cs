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
    [SerializeField] private float hangStaminaCost;
    [SerializeField] private float wallJumpLockDuration;

    [SerializeField] private float midairAcceleration;
    [SerializeField] private float groundDecel;
    [SerializeField] private float midairDecel;

    public float stamina;
    public float sanity;

    public bool canReduceSanity;

    [Header("State")]

    [SerializeField] private CharacterState characterState;
    [SerializeField] private float jumpCount;

    public bool controllable;

    public bool grounded;
    public bool touchingRight;
    public bool touchingLeft;
    public bool hanging;
    public bool wallJumping;

    [Header("Controls")]

    [SerializeField] private KeyCode moveLeft;
    [SerializeField] private KeyCode moveRight;
    [SerializeField] private KeyCode jump;
    [SerializeField] private KeyCode hang;

    [Header("Timers")]
    [SerializeField] private float wallJumpCurrentTimer;

    [Header("Animation Manager")]
    public CharacterAnimation animationManager;

    public void Initialize()
    {

    }

    void Update()
    {
        if (controllable)
        {
            // Detect Inputs
            if (!wallJumping)
            {
                if (Input.GetKey(moveLeft))
                {
                    Move(true, Time.deltaTime);
                }
                else if (Input.GetKey(moveRight))
                {
                    Move(false, Time.deltaTime);
                }
                else
                {
                    // Decel if midair
                    if (!grounded)
                    {
                        if (rb.velocity.x > -0.05f && rb.velocity.x < 0.05f)
                        {
                            rb.velocity = new Vector3(0, rb.velocity.y, 0);
                        }
                        else
                        {
                            if (rb.velocity.x > 0)
                            {
                                rb.velocity = new Vector3(rb.velocity.x - midairDecel * Time.deltaTime, rb.velocity.y, 0);
                            }
                            else
                            {
                                rb.velocity = new Vector3(rb.velocity.x + midairDecel * Time.deltaTime, rb.velocity.y, 0);
                            }
                        }
                    }
                    else
                    {
                        //if (rb.velocity.x > -0.05f || rb.velocity.x < 0.05f)
                        //{
                        //    rb.velocity = new Vector3(0, rb.velocity.y, 0);
                        //}
                        //if (rb.velocity.x > 0)
                        //{
                        //    rb.velocity = new Vector3(rb.velocity.x - groundDecel * Time.deltaTime, rb.velocity.y, 0);
                        //}
                        //else
                        //{
                        //    rb.velocity = new Vector3(rb.velocity.x + groundDecel * Time.deltaTime, rb.velocity.y, 0);
                        //}

                        // Hacky, can't be pushed by environment
                        rb.velocity = new Vector3(0, rb.velocity.y);
                    }

                    animationManager.UpdateAnimatorValue(0);
                }

                if (Input.GetKeyDown(jump) && !hanging)
                {
                    Jump();
                }

                if (Input.GetKey(hang) && stamina > 0)
                {
                    if (!Input.GetKeyDown(jump))
                    {
                        if (touchingLeft)
                        {
                            // Play anim 
                            animationManager.PlayTargetAnimation("WallStick");
                            hanging = true;
                            rb.constraints = RigidbodyConstraints.FreezeAll;
                        }
                        else if (touchingRight)
                        {
                            // Play anim
                            animationManager.PlayTargetAnimation("WallStick");
                            hanging = true;
                            rb.constraints = RigidbodyConstraints.FreezeAll;
                        }
                    }

                    if (Input.GetKeyDown(jump))
                    {
                        WallJump();
                        animationManager.PlayTargetAnimation("WallStickJump");
                    }
                }
                else
                {
                    hanging = false;
                    rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
                }

                // Stamina Regen
                if (grounded && rb.velocity.x < 0.05f && rb.velocity.x > -0.05f)
                {
                    RegenStamina(Time.deltaTime);
                }

                // Hang Stamina
                if (hanging)
                {
                    HangStamina(Time.deltaTime);
                }
            }

            // Wall Jump Timer
            if (wallJumping)
            {
                if (wallJumpCurrentTimer < wallJumpLockDuration)
                {
                    wallJumpCurrentTimer += Time.deltaTime;
                }
                else
                {
                    wallJumping = false;
                    wallJumpCurrentTimer = 0;
                }
            }

            // Jump Physics
            if (rb.velocity.y < 0 && !grounded)
            {
                rb.velocity += (fallSpeed - 1) * Physics.gravity.y * Time.deltaTime * Vector3.up;

                jumpCount = 0;
            }
            else if (rb.velocity.y > 0 && !(Input.GetKey(KeyCode.Space)))
            {
                rb.velocity += (lowJumpMultiplier - 1) * Physics.gravity.y * Time.deltaTime * Vector3.up;
            }

            // Failsafe for buggy interactions
            if (grounded && jumpCount == 0 && !hanging && rb.velocity.y < 0.02f)
            {
                RefreshJump();
            }
            else if (!grounded)
            {
                jumpCount = 0;
            }

            if (canReduceSanity)
            {
                DrainSanity(Time.deltaTime);
            }
        }
    }

    private void StateAssigner(CharacterState newState)
    {
        characterState = newState;
    }

    private void Move(bool left, float deltaTime = 0)
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

        if (grounded)
        {
            if (left)
            {
                rb.velocity = new Vector3(-horizontalSpeed, rb.velocity.y, 0);
                animationManager.FlipModel(-1);
            }
            else
            {
                rb.velocity = new Vector3(horizontalSpeed, rb.velocity.y, 0);
                animationManager.FlipModel(1);
            }

            //
            animationManager.UpdateAnimatorValue(1);
        }
        else
        {
            if (left)
            {
                rb.velocity = new Vector3(rb.velocity.x - midairAcceleration * deltaTime, rb.velocity.y, 0);
            }
            else
            {
                rb.velocity = new Vector3(rb.velocity.x + midairAcceleration * deltaTime, rb.velocity.y, 0);
            }

            if(rb.velocity.x > horizontalSpeed) { rb.velocity = new Vector3(horizontalSpeed, rb.velocity.y, 0); }
            if(rb.velocity.x < -horizontalSpeed) { rb.velocity = new Vector3(-horizontalSpeed, rb.velocity.y, 0); }
        }

    }

    private void WallJump()
    {
        //if(characterState != CharacterState.HANGING)
        //{
        //    return;
        //}

        // Pay stamina first
        if (stamina >= staminaJumpCost)
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
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
            wallJumping = true;
            hanging = false;

            animationManager.PlayTargetAnimation("WallStickJump");
            animationManager.FlipModel(1);
        }
        else if (touchingRight)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(new Vector3(-wallJumpHorizontalForce, wallJumpVerticalForce, 0), ForceMode.Impulse);
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
            wallJumping = true;
            hanging = false;

            animationManager.PlayTargetAnimation("WallStickJump");
            animationManager.FlipModel(-1);
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
        if(stamina >= staminaJumpCost)
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
            animationManager.PlayTargetAnimation("JumpUp");
            animationManager.anim.SetBool("Landed", false);
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

    private void HangStamina(float duration)
    {
        stamina -= duration * hangStaminaCost;

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
