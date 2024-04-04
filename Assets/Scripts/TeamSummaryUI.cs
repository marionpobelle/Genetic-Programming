using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TeamSummaryUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI team;
    [SerializeField] Transform agentDataParent;
    [SerializeField] AgentArchetypeUI agentArchetypePrefab;

    public void SetupTeamDisplay(int teamNumber, List<ValueTuple<float, AgentData>> scores)
    {
        team.text = "Team " + teamNumber.ToString("D2");

        foreach (Transform child in agentDataParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var score in scores)
        {
            AgentArchetypeUI newUI = Instantiate(agentArchetypePrefab, agentDataParent);

            newUI.Setup(score);
        }
    }
}
