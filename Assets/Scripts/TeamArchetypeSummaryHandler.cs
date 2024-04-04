using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamArchetypeSummaryHandler : MonoBehaviour
{
    [SerializeField] TeamArchetypeUI ui;

    bool isUIOpened = false;

    public void SetupUI(int teamAmount)
    {
        ui.SetupUI(teamAmount);
    }

    public void UpdateUI(int teamIndex, List<ValueTuple<float, AgentData>> teamScores)
    {
        ui.UpdateUI(teamIndex, teamScores);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleUI();
        }
    }

    private void ToggleUI()
    {
        isUIOpened = !isUIOpened;

        ui.Toggle(isUIOpened);

        if (isUIOpened)
        {
            Time.timeScale = .01f; 
        }
        else
        {
            Time.timeScale = 1f; 
        }
    }
}
