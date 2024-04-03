using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatsShuffler : MonoBehaviour
{
    [SerializeField] int maxPointsToGive = 100;
    [SerializeField] float bestBuildToKeepPercentage = 10;
    public AgentData GetRandomData()
    {
        AgentData data = new AgentData();

        float hpProportion = UnityEngine.Random.Range(0f, 1f);
        float attackProportion = UnityEngine.Random.Range(0f, 1f);
        float defenseProportion = UnityEngine.Random.Range(0f, 1f);
        float precisionProportion = UnityEngine.Random.Range(0f, 1f);
        float evasivenessProportion = UnityEngine.Random.Range(0f, 1f);
        float attackSpeedProportion = UnityEngine.Random.Range(0f, 1f);
        float attackDistanceProportion = UnityEngine.Random.Range(0f, 1f);

        float proportionsTotal = hpProportion + attackProportion + defenseProportion + precisionProportion + evasivenessProportion + attackSpeedProportion + attackDistanceProportion;


        int hpPoints = Mathf.FloorToInt(maxPointsToGive * Mathf.InverseLerp(0, proportionsTotal, hpProportion));
        data.AdditionalHPPoints = hpPoints;

        int atkPoints = Mathf.FloorToInt(maxPointsToGive * Mathf.InverseLerp(0, proportionsTotal, attackProportion));
        data.AdditionalAttackPoints = atkPoints;

        int defPoints = Mathf.FloorToInt(maxPointsToGive * Mathf.InverseLerp(0, proportionsTotal, defenseProportion));
        data.AdditionalDefensePoints = defPoints;

        int precPoints = Mathf.FloorToInt(maxPointsToGive * Mathf.InverseLerp(0, proportionsTotal, precisionProportion));
        data.AdditionalPrecisionPoints = precPoints;

        int evaPoints = Mathf.FloorToInt(maxPointsToGive * Mathf.InverseLerp(0, proportionsTotal, evasivenessProportion));
        data.AdditionalEvasivenessPoints = evaPoints;

        int atkSpeedPoints = Mathf.FloorToInt(maxPointsToGive * Mathf.InverseLerp(0, proportionsTotal, attackSpeedProportion));
        data.AdditionalAttackSpeedPoints = atkSpeedPoints;

        int distPoints = Mathf.FloorToInt(maxPointsToGive * Mathf.InverseLerp(0, proportionsTotal, attackDistanceProportion));
        data.AdditionalAttackDistancePoints = distPoints;

        //in case we didn't fill up to 100 points
        while (data.StatsTotal < 100)
        {
            int random = UnityEngine.Random.Range(0, 7);

            switch (random)
            {
                case 0:
                    data.AdditionalHPPoints++;
                    break;
                case 1:
                    data.AdditionalAttackPoints++;
                    break;
                case 2:
                    data.AdditionalDefensePoints++;
                    break;
                case 3:
                    data.AdditionalPrecisionPoints++;
                    break;
                case 4:
                    data.AdditionalEvasivenessPoints++;
                    break;
                case 5:
                    data.AdditionalAttackSpeedPoints++;
                    break;
                case 6:
                    data.AdditionalAttackDistancePoints++;
                    break;
            }
        }

        return data;
    }

    public List<ValueTuple<float, AgentData>> GetBestBuilds(List<ValueTuple<float, AgentData>> allBuilds)
    {
        List<ValueTuple<float, AgentData>> bestBuilds = new List<(float, AgentData)>();

        allBuilds = allBuilds.OrderBy(s => s.Item1).ToList();

        int buildsAmountToReturn = Mathf.CeilToInt(allBuilds.Count / bestBuildToKeepPercentage);

        for (int i = 0; i < buildsAmountToReturn; i++)
        {
            bestBuilds.Add(allBuilds[allBuilds.Count - (1 + i)]);
        }

        return bestBuilds;
    }
}
