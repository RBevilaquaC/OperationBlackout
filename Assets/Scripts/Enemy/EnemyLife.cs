using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLife : LifeSystem
{
    #region Parameters

    [SerializeField] private int energyReward;

    #endregion
    
    public void Respawn()
    {
        currentLife = maxLife;
    }

    protected override void Death()
    {
        base.Death();
        GameController.gm.AddEnergy(energyReward);
    }

    public int GetCurrentLife()
    {
        return currentLife;
    }
}
