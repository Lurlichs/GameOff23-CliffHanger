using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallChecker : MonoBehaviour
{
    public bool left;
    [SerializeField] CharacterControl characterControl;


    private void OnTriggerEnter2D(Collider2D collision)
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            if (left)
            {
                characterControl.touchingRight = true;
            }
            else
            {
                characterControl.touchingLeft = true;
            }
        }
    }
}
