using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SanityMechanicController : MonoBehaviour
{
    public Material halluMat;

    public float currentValue;

    [SerializeField] private CharacterControl characterControl;
    [SerializeField] private float currentSanity;

    [SerializeField] private float currentPulsePercentage;
    [SerializeField] private List<Renderer> hallucinationRenderers;

    [SerializeField] private Volume pp;

    private Vignette vig;
    private ChromaticAberration cab;

    private Color hiSanity = new Color(0.3f, 0.2f, 0.33f);
    private Color loSanity = new Color(0.3f, 0f, 0.33f);

    private void Start()
    {
        pp.profile.TryGet(out vig);
        pp.profile.TryGet(out cab);

        StartCoroutine(PulseFresnel());
    }

    private void Update()
    {
        currentSanity = characterControl.sanity;
    }

    private IEnumerator PulseFresnel()
    {
        bool goingUp = false;
        bool waiting = false;

        float maxValue = 10;
        float minValue = 0.5f;

        float maxChromatic = 0.4f;

        float waitTime = 1;

        // sanity becomes a multiplier, from 25 at max sanity to 10 at low sanity
        float maxSpeed = 25;
        float minSpeed = 10;

        float currentWaitTime = 0;
        currentValue = 0;

        while(true){
            if (!waiting)
            {
                float currentSpeed = minSpeed + (maxSpeed - minSpeed) * (currentSanity / 100);
                
                if (goingUp)
                {
                    currentValue += (currentSpeed / 100) * Time.deltaTime; // currval is between 0 and 1

                    if (currentValue >= 1)
                    {
                        waiting = true;
                        goingUp = false;
                    }
                }
                else
                {
                    currentValue -= (currentSpeed / 100) * Time.deltaTime; // currval is between 0 and 1

                    if (currentValue <= 0)
                    {
                        goingUp = true;
                    }
                }
            }
            else
            {
                currentWaitTime += Time.deltaTime;

                if(currentWaitTime > waitTime)
                {
                    currentWaitTime = 0;
                    waiting = false;
                }
            }

            ChangeFresnelValues(currentValue * (maxValue - minValue) + minValue);
            vig.color.value = Color.Lerp(hiSanity, loSanity, (1 - currentSanity / 100));
            cab.intensity.value = (1 - currentSanity / 100) / maxChromatic;

            yield return new WaitForEndOfFrame();
        }
    }

    private void ChangeFresnelValues(float newValue)
    {
        foreach(Renderer renderer in hallucinationRenderers)
        {
            renderer.material.SetFloat("_FresnelMultiplier", newValue);
        }
    }
}
