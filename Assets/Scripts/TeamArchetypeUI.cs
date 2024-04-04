using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamArchetypeUI : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Transform teamArchetypesParent;
    [SerializeField] TeamSummaryUI teamSummaryPrefab;

    List<TeamSummaryUI> teams = new List<TeamSummaryUI>();

    public void Toggle(bool isUIOpened)
    {
        canvasGroup.alpha = isUIOpened ? 1 : 0;
    }

    public void SetupUI(int teamAmount)
    {
        for (int i = 0; i < teamAmount; i++)
        {
            teams.Add(Instantiate(teamSummaryPrefab, teamArchetypesParent));
        }
    }

    public void UpdateUI(int teamIndex, List<(float, AgentData)> teamScores)
    {
        teams[teamIndex].SetupTeamDisplay(teamIndex, teamScores);
    }
}
