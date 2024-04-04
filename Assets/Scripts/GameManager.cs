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
    [SerializeField] TeamArchetypeSummaryHandler teamUIHandler;
    [SerializeField] StatsShuffler shuffler;
    [SerializeField] SimulationSummaryUI simulationSummaryUI;

    int currentGen = 0;
    int lastWinningTeam => combatManager.GetLastWinningTeam();

    void Awake()
    {
        for (int teamIndex = 0; teamIndex < teamAmount; teamIndex++)
        {
            List<Agent> newAgentTeam = new List<Agent>();
            GameObject teamParent = new GameObject($"Team_{teamIndex.ToString("D2")}");
            Material teamMaterial = (teamIndex >= teamMaterials.Count) ? teamMaterials[0] : teamMaterials[teamIndex];

            for (int agentIndex = 0; agentIndex < teamPopulation; agentIndex++)
            {
                Agent agent = Instantiate(agentPrefab, teamParent.transform);

                agent.SetupAgent(teamIndex, teamMaterial, shuffler.GetRandomData());

                newAgentTeam.Add(agent);
            }

            combatManager.AddTeam(newAgentTeam);
        }

        teamUIHandler.SetupUI(teamAmount);

        StartFight();
    }

    void StartFight()
    {
        currentGen++;
        simulationSummaryUI.UpdateUI(currentGen, lastWinningTeam);
        combatManager.StartFight(OnFightOver);
    }

    void OnFightOver(List<List<ValueTuple<float, AgentData>>> teamsPerformances)
    {
        foreach (var perf in teamsPerformances)
        {
            combatManager.UpdateTeamStrategy(teamsPerformances.IndexOf(perf), shuffler.GetBestBuilds(perf));
            teamUIHandler.UpdateUI(teamsPerformances.IndexOf(perf), shuffler.GetBestBuilds(perf));
        }

        combatManager.ResetAllAgents();

        StartFight();
    }
}
