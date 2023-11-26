using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HallucinationFadeTrigger : MonoBehaviour
{
    [SerializeField] private GameObject platformModel;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = platformModel.GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        meshRenderer.material.DOFade(0, 1).OnComplete(() =>
        {
            meshRenderer.gameObject.SetActive(false);
        }); 
    }
}
