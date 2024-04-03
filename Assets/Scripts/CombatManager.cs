using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] float arenaDimension;
    Action<List<List<ValueTuple<float, AgentData>>>> fightOverCallback;

    public List<List<Agent>> teams = new List<List<Agent>>();

    //BUG: Draw event stuff
    float lastOnHitTime;
    //BUG: Draw event stuff
    float maxTimeBetweenOnHits = 60.0f;

    public static CombatManager Instance;

    private void Awake()
    {
        Instance = this;
    }

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
        //Subscribe to agent death to check for fight over
        Agent.OnDeath += OnAgentDeath;
        //Subscribe to agent hit to check for draws
        //BUG: Draw event stuff
        lastOnHitTime = Time.time;
        Agent.OnHit += OnAgentHit;
        
    }

    //BUG: Draw event stuff
    private void OnAgentHit()
    {
        Debug.Log("Calling OnAgentHit!");
        lastOnHitTime = Time.time;
    }

    private void Update()
    {
        //BUG: Draw event stuff
        if (Mathf.Abs(lastOnHitTime - Time.time) >= maxTimeBetweenOnHits)
        {
            Debug.Log("Hit cooldown reach ! Calling EndFight");
            EndFight();
        }
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

        Agent.OnDeath -= OnAgentDeath;
        Agent.OnHit -= OnAgentHit;
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
