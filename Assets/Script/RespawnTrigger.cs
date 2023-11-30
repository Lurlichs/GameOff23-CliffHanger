using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{

    [SerializeField] private GameObject campfireParticles;
    [SerializeField] private Transform respawnPos;
    [SerializeField] private int priority;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterControl>() != null)
        {
            campfireParticles.SetActive(true);

            if(ValueTracker.Instance != null)
            {
                ValueTracker.Instance.SetNewRespawnCoords(respawnPos.position, priority);
            }
            else
            {
                Debug.LogWarning("Respawn tracker missing");
            }
        }
    }
}
