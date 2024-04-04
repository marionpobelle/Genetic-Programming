using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class StatPanelDisplayHandler : MonoBehaviour
{
    Camera mainCamera;
    List<Agent> allAgents;
    int displayAgentIndex = 0;

    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] TextMeshProUGUI Teamtxt;
    [SerializeField] TextMeshProUGUI MaxHPtxt;
    [SerializeField] TextMeshProUGUI Attacktxt;
    [SerializeField] TextMeshProUGUI Defensetxt;
    [SerializeField] TextMeshProUGUI Evasivenesstxt;
    [SerializeField] TextMeshProUGUI Precisiontxt;
    [SerializeField] TextMeshProUGUI AttackSpeedtxt;
    [SerializeField] TextMeshProUGUI AttackDistancetxt;
    [SerializeField] Slider atkSlider;
    [SerializeField] Slider defSlider;
    [SerializeField] Slider precSlider;
    [SerializeField] Slider evaSlider;
    [SerializeField] Slider aspSlider;
    [SerializeField] Slider araSlider;
    [SerializeField] Slider hpSlider;

    [SerializeField] TextMeshProUGUI Killtxt;
    [SerializeField] TextMeshProUGUI Damagetxt;

    private void Start()
    {
        mainCamera = Camera.main;
        allAgents = new List<Agent>();
        for (int i = 0; i < CombatManager.Instance.teams.Count; i++) 
        { 
            foreach(Agent agent in CombatManager.Instance.teams[i])
            {
                allAgents.Add(agent);
            }
        }
        SetStatsPanel(allAgents[0]);
        displayAgentIndex = 0;
        Toggle(false);
    }

    void SetStatsPanel(Agent agent)
    {
        Teamtxt.SetText("Team : " + agent.teamIndex);

        //MaxHPtxt.SetText("MaxHP : " + agent.Data.MaxHP);
        //Attacktxt.SetText("Atk : " + agent.Data.Attack);
        //Defensetxt.SetText("Def : " + agent.Data.Defense);
        //Evasivenesstxt.SetText("Evs : " + agent.Data.Evasiveness);
        //Precisiontxt.SetText("Pre : " + agent.Data.Precision);
        //AttackSpeedtxt.SetText("Atk Speed : " + agent.Data.Speed);
        //AttackDistancetxt.SetText("Atk Dist : " + agent.Data.AttackDistance);

        Killtxt.SetText("Kills : " + agent.KillAmount);
        Damagetxt.SetText("Dmg : " + agent.TotalDamageInflicted);
    }

    private void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out RaycastHit hit))
        {
            Agent agent = hit.collider.GetComponent<Agent>();
            if (agent != null && agent.IsAlive && allAgents.Contains(agent))
            {
                displayAgentIndex = allAgents.IndexOf(agent);
                Toggle(true);
            }
            else if (Input.GetMouseButtonDown(0))
                Toggle(false);
        }
        else if (Input.GetMouseButtonDown(0))
                Toggle(false);


        if (Input.GetKeyDown(KeyCode.Q))
        {
            displayAgentIndex -= 1;
            if (displayAgentIndex >= allAgents.Count) displayAgentIndex = 0;
            else if (displayAgentIndex < 0) displayAgentIndex = allAgents.Count - 1;
            while(allAgents[displayAgentIndex].IsAlive == false)
            {
                displayAgentIndex--;
                if (displayAgentIndex < 0) displayAgentIndex = allAgents.Count - 1;
            }
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            displayAgentIndex += 1;
            if (displayAgentIndex >= allAgents.Count) displayAgentIndex = 0;
            else if (displayAgentIndex < 0) displayAgentIndex = allAgents.Count - 1;
            while (allAgents[displayAgentIndex].IsAlive == false)
            {
                displayAgentIndex++;
                if (displayAgentIndex >= allAgents.Count) displayAgentIndex = 0;
            }
        }

        if (allAgents[displayAgentIndex].IsAlive == false)
        {
            Toggle(false);
            
        }
        transform.position = allAgents[displayAgentIndex].transform.position + new Vector3(0, 8, 0);
        SetStatsPanel(allAgents[displayAgentIndex]);

        if (allAgents[displayAgentIndex].IsAlive)
        {
            atkSlider.value = allAgents[displayAgentIndex].Data.AdditionalAttackPoints;
            defSlider.value = allAgents[displayAgentIndex].Data.AdditionalDefensePoints;
            precSlider.value = allAgents[displayAgentIndex].Data.AdditionalPrecisionPoints;
            evaSlider.value = allAgents[displayAgentIndex].Data.AdditionalEvasivenessPoints;
            aspSlider.value = allAgents[displayAgentIndex].Data.AdditionalSpeedPoints;
            araSlider.value = allAgents[displayAgentIndex].Data.AdditionalAttackDistancePoints;
            hpSlider.value = allAgents[displayAgentIndex].Data.AdditionalHPPoints;
        }
    }


    void Toggle(bool show)
    {
        canvasGroup.alpha = show ? 1 : 0;
    }
}


