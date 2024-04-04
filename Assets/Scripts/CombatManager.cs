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

    float fightStartTime;
    float fightEndTime;

    //BUG: Draw event stuff
    float lastOnHitTime;
    //BUG: Draw event stuff
    float maxTimeBetweenOnHits = 60.0f;

    public static CombatManager Instance;

    bool isFightRunning = false;

    public bool IsFightRunning => isFightRunning;

    int lastWinningTeam = -1;

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
        isFightRunning = true;
        //Subscribe to agent hit to check for draws
        //BUG: Draw event stuff
        lastOnHitTime = Time.time;
        Agent.OnHit += OnAgentHit;
        fightStartTime = Time.time;
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

        int possiblyWinningTeam = -1;
        foreach (List<Agent> team in teams)
        {
            foreach (Agent agent in team)
            {
                if (agent.IsAlive)
                {
                    teamsWithLivingPlayerAlive++;
                    possiblyWinningTeam = teams.IndexOf(team);
                    break;
                }
            }

            if (teamsWithLivingPlayerAlive >= 2)
            {
                break;
            }
        }

        if (teamsWithLivingPlayerAlive <= 1)
        {
            lastWinningTeam = possiblyWinningTeam;
            EndFight();
            return;
        }
    }

    private void EndFight()
    {
        isFightRunning = false;

        fightEndTime = Time.time;

        //compute scores
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

    public float ComputeDeathLifetimePercentage(float deathTime)
    {
        return 100 * Mathf.InverseLerp(fightStartTime, fightEndTime, deathTime);
    }

    public void ResetAllAgents()
    {
        foreach (List<Agent> team in teams)
        {
            foreach (Agent agent in team)
            {
                agent.InitAgent(GetRandomArenaPosition());
            }
        }
    }

    public void UpdateTeamStrategy(int teamIndex, List<ValueTuple<float, AgentData>> possibleBuilds)
    {
        foreach (Agent agent in teams[teamIndex])
        {
            //one random strategy per team
            if (teams[teamIndex].IndexOf(agent) == 0)
            {
                agent.InitAgent(GetRandomArenaPosition());
                continue;
            }

            int chosenStrategy = UnityEngine.Random.Range(0, possibleBuilds.Count);
            agent.UpdateData(possibleBuilds[chosenStrategy].Item2, possibleBuilds[chosenStrategy].Item1);
        }
    }

    public int GetLastWinningTeam()
    {
        return lastWinningTeam;
    }
}
