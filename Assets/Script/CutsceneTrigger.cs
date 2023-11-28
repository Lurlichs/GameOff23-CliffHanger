using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public string[] cutsceneLines;

    [SerializeField] private bool isSanityCutscene;

    private void Start()
    {
        //HUD.instance.BeginCutscene(cutsceneLines);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterControl>() != null)
        {
            HUD.instance.BeginCutscene(cutsceneLines, true);
            gameObject.SetActive(false);

            if(isSanityCutscene == true)
            {
                HUD.instance.sanityCanvasGroup.alpha = 1;
            }
        }
    }
}
