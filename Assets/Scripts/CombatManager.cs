using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] float arenaDimension;
    Action<List<List<ValueTuple<float, AgentData>>>> fightOverCallback;

    public List<List<Agent>> teams = new List<List<Agent>>();

    public static CombatManager Instance;

    public void AddTeam(List<Agent> newTeam)
    {
        teams.Add(newTeam);
    }

    public void StartFight(Action<List<List<ValueTuple<float, AgentData>>>> fightOverCallback)
    {
        this.fightOverCallback = fightOverCallback;

        foreach (List<Agent> team in teams)
        {
            foreach (Agent agent in team)
            {
                agent.InitAgent(GetRandomArenaPosition());
            }
        }
        //subscribe to agent death to check for fight over
        Agent.OnDeath += OnAgentDeath;
    }

    void OnAgentDeath()
    {
        int teamsWithLivingPlayerAlive = 0;
        foreach (List<Agent> team in teams)
        {
            foreach (Agent agent in team)
            {
                if (agent.IsAlive)
                {
                    teamsWithLivingPlayerAlive++;
                    break;
                }
            }

            if (teamsWithLivingPlayerAlive <= 1)
            {
                EndFight();
                return;
            }
        }
    }

    private void EndFight()
    {
        List<List<ValueTuple<float, AgentData>>> teamsScores = new List<List<(float, AgentData)>>();

        foreach (List<Agent> team in teams)
        {
            teamsScores.Add(GetTeamScore(team));
        }

        fightOverCallback?.Invoke(teamsScores);
    }

    List<(float, AgentData)> GetTeamScore(List<Agent> team)
    {
        List<ValueTuple<float, AgentData>> teamScore = new List<ValueTuple<float, AgentData>>();

        foreach (Agent agent in team)
        {
            teamScore.Add(new(agent.ComputeScore(), agent.Data));
        }

        return teamScore;
    }

    private Vector3 GetRandomArenaPosition()
    {
        return new Vector3(UnityEngine.Random.Range(-arenaDimension, arenaDimension), 0, UnityEngine.Random.Range(-arenaDimension, arenaDimension));
    }
}
