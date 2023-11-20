using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    public Animator anim;
    int idleRunBlend;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    #region Animator references
    //Triggers
    //-------------
    //StandUp
    //JumpPeaked
    //Landed

    //Animations
    //-------------
    //JumpUp
    //WallStick
    //WallStickJump
    //Ledge
    //Die
    //SanityMid
    //SanityHigh
    #endregion

    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        anim.CrossFade(targetAnim, 0.2f);
    }

    public void UpdateAnimatorValue(float moveAmount)
    {
        float v = 0;

        if(moveAmount > 0)
        {
            v = 1;
        }
        else
        {
            v = 0;
        }

        anim.SetFloat(idleRunBlend, v, 0.1f, Time.deltaTime);
    }
}
