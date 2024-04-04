using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SimulationSummaryUI : MonoBehaviour
{
    bool isUIShown = true;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] TextMeshProUGUI gen;
    [SerializeField] TextMeshProUGUI elapsedTimeLabel;
    [SerializeField] TextMeshProUGUI lastWin;
    [SerializeField] KeyCode toggleKey;

    float elapsedTime = 0f;

    private void Update()
    {
        if(Input.GetKeyDown(toggleKey)) 
        {
            isUIShown = !isUIShown;

            canvasGroup.alpha = isUIShown ? 1f : 0f;
        }

        elapsedTime += Time.deltaTime;
        elapsedTimeLabel.text = elapsedTime.ToString("F2");
    }

    public void UpdateUI(int genAmount, int lastWinningTeam)
    {
        elapsedTime = 0f;
        gen.text = genAmount.ToString();
        lastWin.text = "Team " + lastWinningTeam.ToString();
    }
}
