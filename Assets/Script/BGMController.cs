using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    public static BGMController Instance;
    public int currentIndex;

    public List<AudioClip> audioClips;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayNextStage()
    {
        switch (currentIndex)
        {
            case 0:
                audioSource.clip = audioClips[0];
                break;
            case 1:
                audioSource.clip = audioClips[1];
                break;
            case 2:
                audioSource.clip = audioClips[2];
                break;
            default:
                break;
        }

        audioSource.Play();
    }
}
