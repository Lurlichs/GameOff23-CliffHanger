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
public class CharacterController : MonoBehaviour
{
    [Header("Stats")]

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

    public void Initialize()
    {

    }

    void Update()
    {
        // Detect Inputs
    }

    private void StateAssigner()
    {

    }

    private void Move(bool left)
    {
        List<CharacterState> validStates = new List<CharacterState> {
            CharacterState.STANDING,
            CharacterState.RUNNING,
            CharacterState.ON_LEDGE
        };

        // Can't move in this situation
        if (!validStates.Contains(characterState))
        {
            return;
        }
    }

    private void Jump()
    {
        List<CharacterState> validStates = new List<CharacterState> { 
            CharacterState.STANDING,
            CharacterState.RUNNING,
            CharacterState.HANGING,
            CharacterState.ON_LEDGE
        };

        // Can't jump in this situation
        if (!validStates.Contains(characterState))
        {
            return;
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
        }

        if (characterState == CharacterState.HANGING)
        {
            // Wall jump
        }
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
