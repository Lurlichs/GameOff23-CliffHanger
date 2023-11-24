using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public string[] cutsceneLines;

    private void Start()
    {
        //HUD.instance.BeginCutscene(cutsceneLines);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterControl>() != null)
        {
            HUD.instance.BeginCutscene(cutsceneLines);
            gameObject.SetActive(false);
        }
    }
}
