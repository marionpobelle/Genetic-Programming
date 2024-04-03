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
}
