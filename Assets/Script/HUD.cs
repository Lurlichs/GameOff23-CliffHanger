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
    public CanvasGroup sanityCanvasGroup;

    [SerializeField] private CharacterControl characterControl;

    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        sanityCanvasGroup.alpha = 0;
        HUDGroup.alpha = 0;
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

    public void BeginCutscene(string[] cutsceneLines, bool disableCharacterMovement)
    {
        cutsceneManager.lines = cutsceneLines;
        HUDGroup.DOFade(0, 0.5f);
        cutsceneBars.DOScale(1, 1).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            cutsceneManager.StartCutsceneText();
        });

        if (disableCharacterMovement)
        {
            characterControl.SetControllable(false);
        }
    }

    public void EndCutscene()
    {
        HUDGroup.DOFade(1, 0.25f);
        cutsceneBars.DOScale(1.4f, 0.5f).SetEase(Ease.OutQuad);
        characterControl.SetControllable(true);
        characterControl.animationManager.anim.SetTrigger("StandUp");
    }

    
}
