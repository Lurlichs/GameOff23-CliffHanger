using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    [SerializeField] private CharacterControl characterControl;
    [SerializeField] private Image blackScreen;

    private void Awake()
    {
        Instance = this;
        StartCoroutine(InTransition());
    }

    private void Start()
    {
        ValueTracker.Instance.ResetScene();

        if (ValueTracker.Instance.cutscenePriority != 0)
        {
            HUD.instance.CallPlayerStand();
        }
    }

    public void Death()
    {
        StartCoroutine(DeathTransition());
    }
    
    public void NextStage(string scene)
    {
        StartCoroutine(NextStageTransition(scene));
    }

    private IEnumerator DeathTransition()
    {
        float time = 0.5f;
        float finalWaitTime = 0.15f;
        float currentTime = 0f;

        while(currentTime < time)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, currentTime / time);

            currentTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(finalWaitTime);

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    private IEnumerator NextStageTransition(string scene)
    {
        float time = 0.5f;
        float finalWaitTime = 1f;
        float currentTime = 0f;

        while (currentTime < time)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, currentTime / time);

            currentTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(finalWaitTime);

        SceneManager.LoadScene(scene);
    }

    private IEnumerator InTransition()
    {
        float time = 0.5f;
        float currentTime = 0f;

        while (currentTime < time)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 1 - currentTime / time);

            currentTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 0);
    }
}
