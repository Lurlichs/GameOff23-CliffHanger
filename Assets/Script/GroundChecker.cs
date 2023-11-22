using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] CharacterControl characterControl;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Wall"))
        {
            characterControl.RefreshJump();
            characterControl.grounded = true;
            characterControl.animationManager.anim.SetBool("Landed", true);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Wall"))
        {
            characterControl.grounded = false;
            characterControl.animationManager.anim.SetBool("Landed", false);
            characterControl.animationManager.PlayTargetAnimation("JumpUp");
        }
    }
}
