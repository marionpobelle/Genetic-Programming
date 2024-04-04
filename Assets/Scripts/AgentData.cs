using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AgentData
{
    int baseMaxHP = 100;
    int baseAttack = 10;
    int baseDefense = 1;
    int baseEvasiveness = 1;
    int basePrecision = 5;
    int baseSpeed = 5;
    float baseAttackDistance = 1;

    public int AdditionalHPPoints = 0;
    public int AdditionalAttackPoints = 0;
    public int AdditionalDefensePoints = 0;
    public int AdditionalEvasivenessPoints = 0;
    public int AdditionalPrecisionPoints = 0;
    public int AdditionalSpeedPoints = 0;
    public int AdditionalAttackDistancePoints = 0;

    int multiplierHP = 3;
    float multiplierAttackDistance = 0.1f;

    public int MaxHP => baseMaxHP + AdditionalHPPoints * multiplierHP;
    public int Attack => baseAttack + AdditionalAttackPoints;
    public int Defense => baseDefense + AdditionalDefensePoints;
    public int Evasiveness => baseEvasiveness + AdditionalEvasivenessPoints;
    public int Precision => basePrecision + AdditionalPrecisionPoints;
    public int Speed => baseSpeed + AdditionalSpeedPoints;
    public float AttackDistance => baseAttackDistance + AdditionalAttackDistancePoints * multiplierAttackDistance;

    public int StatsTotal => AdditionalHPPoints + AdditionalAttackPoints + AdditionalDefensePoints + AdditionalEvasivenessPoints + AdditionalPrecisionPoints + AdditionalSpeedPoints +AdditionalAttackDistancePoints;


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
        AdditionalSpeedPoints = newData.AdditionalSpeedPoints;
        AdditionalAttackDistancePoints = newData.AdditionalAttackDistancePoints;

        int pointsToShuffle = Mathf.CeilToInt(Mathf.Lerp(60,5, Mathf.InverseLerp(0,700,buildFitness)));

        while (pointsToShuffle > 0)
        {

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
                    if (AdditionalSpeedPoints <= 0)
                        continue;
                    AdditionalSpeedPoints--;
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
                    AdditionalSpeedPoints++;
                    break;
                case 6:
                    AdditionalAttackDistancePoints++;
                    break;
            }
            pointsToShuffle--;
        }
    }

    public AgentData() { }
}
