using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    public Text cutsceneText;
    public string[] lines;
    [SerializeField] private float textSpeed;

    private int index;
    [HideInInspector] public bool cutsceneActive;

    void Start()
    {
        cutsceneText.text = string.Empty;
    }

    void Update()
    {
        if(cutsceneActive == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (cutsceneText.text == lines[index])
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    cutsceneText.text = lines[index];
                }
            }
        }
    }

    public void StartCutsceneText()
    {
        cutsceneActive = true;
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach(char c in lines[index].ToCharArray())
        {
            cutsceneText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if(index < lines.Length - 1)
        {
            index++;
            cutsceneText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            cutsceneText.text = string.Empty;
            cutsceneActive = false;
            HUD.instance.EndCutscene();
        }
    }
}
