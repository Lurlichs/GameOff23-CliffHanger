using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTracker : MonoBehaviour
{
    public static RespawnTracker Instance;

    [SerializeField] private Vector3 respawnPos;
    [SerializeField] private int currentPriority;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ResetScene()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
    }

    public void SetNewRespawnCoords(Vector3 position, int priority)
    {
        if(priority >= currentPriority)
        {
            respawnPos = position;
            currentPriority = priority;
        }

    }

    public Vector3 GetRespawnCoords()
    {
        return respawnPos;
    }
}
