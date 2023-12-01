using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SFX
{
    public AudioClip audioClip;
    public string clipName;
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private List<SFX> sounds;
    [SerializeField] private AudioSource source;

    void Awake()
    {
        Instance = this;
    }

    public void Play(string Name, float vol = 0.05f)
    {
        source.volume = vol;

        foreach (SFX clip in sounds)
        {
            if (clip.clipName == Name)
            {
                source.PlayOneShot(clip.audioClip);
                break;
            }
        }
    }
}
