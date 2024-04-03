using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField] Renderer agentRenderer;

    /// <summary>
    /// Called when the agent is instantiated
    /// </summary>
    /// <param name="teamMaterial"></param>
    public void SetupAgent(Material teamMaterial)
    {
        agentRenderer.material = teamMaterial;
    }

    /// <summary>
    /// Called when the fight starts
    /// </summary>
    /// <param name="initialPosition"></param>
    public void InitAgent(Vector3 initialPosition)
    {
        transform.position = initialPosition;
    }
}
