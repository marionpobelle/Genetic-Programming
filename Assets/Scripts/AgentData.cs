using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AgentData
{
    int baseMaxHP = 100;
    int baseAttack = 5;
    int baseDefense = 1;
    int baseEvasiveness = 1;
    int basePrecision = 5;
    int baseAttackSpeed = 1;
    float baseAttackDistance = 1;

    public int AdditionalHPPoints = 0;
    public int AdditionalAttackPoints = 0;
    public int AdditionalDefensePoints = 0;
    public int AdditionalEvasivenessPoints = 0;
    public int AdditionalPrecisionPoints = 0;
    public int AdditionalAttackSpeedPoints = 0;
    public int AdditionalAttackDistancePoints = 0;

    int multiplierHP = 3;
    float multiplierAttackDistance = 0.5f;

    public int MaxHP => baseMaxHP + AdditionalHPPoints * multiplierHP;
    public int Attack => baseAttack + AdditionalAttackPoints;
    public int Defense => baseDefense + AdditionalDefensePoints;
    public int Evasiveness => baseEvasiveness + AdditionalEvasivenessPoints;
    public int Precision => basePrecision + AdditionalPrecisionPoints;
    public int AttackSpeed => baseAttackSpeed + AdditionalAttackSpeedPoints;
    public float AttackDistance => baseAttackDistance + AdditionalAttackDistancePoints * multiplierAttackDistance;

    public int StatsTotal => AdditionalHPPoints + AdditionalAttackPoints + AdditionalDefensePoints + AdditionalEvasivenessPoints + AdditionalPrecisionPoints + AdditionalAttackSpeedPoints +AdditionalAttackDistancePoints;


    /// <summary>
    /// constructor used to shuffle
    /// </summary>
    public AgentData(AgentData newData, float buildFitness)
    {
        AdditionalHPPoints = newData.AdditionalHPPoints;
        AdditionalAttackPoints = newData.AdditionalAttackPoints;
        AdditionalDefensePoints = newData.AdditionalDefensePoints;
        AdditionalEvasivenessPoints = newData.AdditionalEvasivenessPoints;
        AdditionalPrecisionPoints = newData.AdditionalPrecisionPoints;
        AdditionalAttackSpeedPoints = newData.AdditionalAttackSpeedPoints;
        AdditionalAttackDistancePoints = newData.AdditionalAttackDistancePoints;

        int pointsToShuffle = Mathf.CeilToInt(Mathf.Lerp(60,5, Mathf.InverseLerp(0,300,buildFitness)));

        while (pointsToShuffle > 0)
        {
            pointsToShuffle--;

            int random = UnityEngine.Random.Range(0, 7);

            switch (random)
            {
                case 0:
                    if (AdditionalHPPoints <= 0)
                        continue;
                    AdditionalHPPoints--;
                    break;
                case 1:
                    if (AdditionalAttackPoints <= 0)
                        continue;
                    AdditionalAttackPoints--;
                    break;
                case 2:
                    if (AdditionalDefensePoints <= 0)
                        continue;
                    AdditionalDefensePoints--;
                    break;
                case 3:
                    if (AdditionalPrecisionPoints <= 0)
                        continue;
                    AdditionalPrecisionPoints--;
                    break;
                case 4:
                    if (AdditionalEvasivenessPoints <= 0)
                        continue;
                    AdditionalEvasivenessPoints--;
                    break;
                case 5:
                    if (AdditionalAttackSpeedPoints <= 0)
                        continue;
                    AdditionalAttackSpeedPoints--;
                    break;
                case 6:
                    if (AdditionalAttackDistancePoints <= 0)
                        continue;
                    AdditionalAttackDistancePoints--;
                    break;
            }

            random = UnityEngine.Random.Range(0, 7);

            switch (random)
            {
                case 0:
                    AdditionalHPPoints++;
                    break;
                case 1:
                    AdditionalAttackPoints++;
                    break;
                case 2:
                    AdditionalDefensePoints++;
                    break;
                case 3:
                    AdditionalPrecisionPoints++;
                    break;
                case 4:
                    AdditionalEvasivenessPoints++;
                    break;
                case 5:
                    AdditionalAttackSpeedPoints++;
                    break;
                case 6:
                    AdditionalAttackDistancePoints++;
                    break;
            }
        }
    }

    public AgentData() { }
}
