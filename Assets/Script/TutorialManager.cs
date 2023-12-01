using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    [SerializeField] private CanvasGroup toMove;

    void Awake()
    {
        Instance = this;
    }
    public void FadeIn()
    {
        toMove.DOFade(1, 2);
    }
}
