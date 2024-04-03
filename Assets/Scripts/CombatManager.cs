using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] float arenaSize;

    List<List<Agent>> teams;

    public void AddTeam(List<Agent> newTeam)
    {
        teams.Add(newTeam);
    }

    public void StartFight()
    {
        throw new NotImplementedException();
    }
}
