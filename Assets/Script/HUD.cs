using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD instance;

    [SerializeField] private Slider staminaSlider;
    [SerializeField] private Slider sanitySlider;

    void Awake()
    {
        instance = this;
    }

    public void SetMaxValues(float maxStam, float maxSan)
    {
        staminaSlider.maxValue = maxStam;
        sanitySlider.maxValue = maxSan;
    }
    
    public void UpdateStaminaDisplay(float val)
    {
        staminaSlider.value = val;
    }

    public void UpdateSanityDisplay(float val)
    {
        sanitySlider.value = val;
    }

}
