using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AgentArchetypeUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreUI;
    [SerializeField] Slider atkSlider;
    [SerializeField] Slider defSlider;
    [SerializeField] Slider precSlider;
    [SerializeField] Slider evaSlider;
    [SerializeField] Slider aspSlider;
    [SerializeField] Slider araSlider;
    [SerializeField] Slider hpSlider;

    public void Setup((float, AgentData) score)
    {
        scoreUI.text = score.Item1.ToString();
        atkSlider.value = score.Item2.AdditionalAttackPoints;
        defSlider.value = score.Item2.AdditionalDefensePoints;
        precSlider.value = score.Item2.AdditionalPrecisionPoints;
        evaSlider.value = score.Item2.AdditionalEvasivenessPoints;
        aspSlider.value = score.Item2.AdditionalAttackSpeedPoints;
        araSlider.value = score.Item2.AdditionalAttackDistancePoints;
        hpSlider.value = score.Item2.AdditionalHPPoints;
    }
}
