using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    [Header("Score Settings")]
    [SerializeField] float killScoreMultiplier = 100;
    [SerializeField] float damageScoreMultiplier = 1;
    [SerializeField] float aliveScoreMultiplier = 1;
    [SerializeField] Renderer agentRenderer;

    /// <summary>
    /// Called when the agent is instantiated
    /// </summary>
    public void SetupAgent(int teamIndex, Material teamMaterial)
    {
        Debug.LogWarning("TODO : SET TEAM INDEX");
        agentRenderer.material = teamMaterial;
    }

    /// <summary>
    /// Called when the fight starts
    /// </summary>
    public void InitAgent(Vector3 initialPosition)
    {
        transform.position = initialPosition;
    }

    public float ComputeScore()
    {
        Debug.Log("TODO : COMPUTE SCORE");

        //function should be something like :
        // return kills * killscoreMultiplier + percentageAliveInGame * aliveScoreMultiplier + totalInflictedDamage * damageScoreMultiplier;

        return 0;
    }
}
