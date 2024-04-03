using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Agent agentPrefab;
    [Header("Game Parameters")]
    [SerializeField] int teamPopulation = 50;
    [SerializeField] int teamAmount = 2;
    [SerializeField] List<Material> teamMaterials;

    [Header("Needed Variables")]
    [SerializeField] CombatManager combatManager;


    void Awake()
    {
        for (int teamIndex = 0; teamIndex < teamAmount; teamIndex++)
        {
            List<Agent> newAgentTeam = new List<Agent>();

            Material teamMaterial = (teamMaterials.Count >= teamIndex) ? teamMaterials[0] : teamMaterials[teamIndex];
            for (int agentIndex = 0; agentIndex < teamPopulation; agentIndex++)
            {
                Agent agent = Instantiate(agentPrefab);

                agent.SetupAgent(teamMaterial);

                newAgentTeam.Add(agent);
            }

            combatManager.AddTeam(newAgentTeam);
        }

        StartFight();
    }

    void StartFight()
    {
        combatManager.StartFight();
    }

    void OnFightOver()
    {

    }
}
