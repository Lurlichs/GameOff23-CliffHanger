using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallChecker : MonoBehaviour
{
    public bool left;
    [SerializeField] CharacterControl characterControl;


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Wall"))
        {
            if (left)
            {
                characterControl.touchingLeft = true;
            }
            else
            {
                characterControl.touchingRight = true;
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Wall"))
        {
            if (left)
            {
                characterControl.touchingLeft = false;
            }
            else
            {
                characterControl.touchingRight = false;
            }
        }
    }
}
