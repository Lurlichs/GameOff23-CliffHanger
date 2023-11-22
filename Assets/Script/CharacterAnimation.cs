using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [HideInInspector] public Animator anim;
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

    public void PlayTargetAnimation(string targetAnim)
    {
        anim.CrossFade(targetAnim, 0.01f);
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

        anim.SetFloat("IdleRunBlend", v, 0.2f, Time.deltaTime);
    }

    public void FlipModel(float direction)
    {
        gameObject.transform.localScale = new Vector3(1, 1, direction);
    }
}
