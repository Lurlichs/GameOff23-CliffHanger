using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class NextStage : MonoBehaviour
{
    public string nextStage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterControl>() != null)
        {
            SceneTransitionManager.Instance.NextStage(nextStage);
        }
    }
}
