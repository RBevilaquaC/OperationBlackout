using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerMonitor : MonoBehaviour
{
    public TMP_Text energyAmount;
    public TMP_Text stageCountText;

    private void Start()
    {
        stageCountText.text = "Current Stage: " + GameController.gm.stageCount.ToString();
    }

    private void FixedUpdate()
    {
        UpdateEnergyAmount();
    }

    private void UpdateEnergyAmount()
    {
        energyAmount.text = GameController.gm.currentEnergy.ToString();
    }
}
