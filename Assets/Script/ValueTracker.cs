using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueTracker : MonoBehaviour
{
    public static ValueTracker Instance;

    [SerializeField] private Vector3 respawnPos;
    [SerializeField] private int currentPriority;

    public int cutscenePriority;

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
        if(respawnPos != Vector3.zero)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = respawnPos;
        }
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
