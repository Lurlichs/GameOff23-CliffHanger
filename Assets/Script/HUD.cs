using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HUD : MonoBehaviour
{
    public static HUD instance;
    public CutsceneManager cutsceneManager;

    [SerializeField] private Slider staminaSlider;
    [SerializeField] private Slider sanitySlider;

    [SerializeField] private CanvasGroup HUDGroup;
    [SerializeField] private Transform cutsceneBars;

    void Awake()
    {
        instance = this;
    }

    public void SetMaxValues(float maxStam, float maxSan)
    {
        staminaSlider.maxValue = maxStam;
        sanitySlider.maxValue = maxSan;
        
        UpdateStaminaDisplay(maxStam);
        UpdateSanityDisplay(maxSan);
    }
    
    public void UpdateStaminaDisplay(float val)
    {
        staminaSlider.value = val;
    }

    public void UpdateSanityDisplay(float val)
    {
        sanitySlider.value = val;
    }

    public void BeginCutscene(string[] cutsceneLines)
    {
        cutsceneManager.lines = cutsceneLines;
        HUDGroup.DOFade(0, 0.5f);
        cutsceneBars.DOScale(1, 1).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            cutsceneManager.StartCutsceneText();
        });
    }

    public void EndCutscene()
    {
        HUDGroup.DOFade(1, 0.25f);
        cutsceneBars.DOScale(1.4f, 0.5f).SetEase(Ease.OutQuad);
    }

    
}
