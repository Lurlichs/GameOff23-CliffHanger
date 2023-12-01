using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class EndTrigger : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CharacterControl characterControl;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterControl>() != null)
        {
            StartCoroutine(InTransition());
        }
    }

    private IEnumerator InTransition()
    {
        characterControl.SetControllable(false);

        float time = 2f;
        float currentTime = 0f;

        while (currentTime < time)
        {
            canvasGroup.alpha = currentTime / time;

            currentTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
