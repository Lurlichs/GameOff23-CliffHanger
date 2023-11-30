using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public string[] cutsceneLines;
    public int cutscenePriority;

    [SerializeField] private bool isSanityCutscene;

    private void Start()
    {
        //HUD.instance.BeginCutscene(cutsceneLines);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterControl>() != null)
        {
            if(cutscenePriority > ValueTracker.Instance.cutscenePriority)
            {
                HUD.instance.BeginCutscene(cutsceneLines, true);
                ValueTracker.Instance.cutscenePriority = cutscenePriority;
                gameObject.SetActive(false);

                if (isSanityCutscene == true)
                {
                    HUD.instance.ActivateSanityBar();
                }
            }
        }
    }
}
