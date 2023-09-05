using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currentEnergy;
    public int[] weapons;
    public int[] auxGad;
    public int wagons;
    public int stageCount;

    public GameData()
    {
        currentEnergy = GameController.gm.currentEnergy;
        wagons = GameController.gm.wagonCount;
        stageCount = GameController.gm.stageCount;
        
        List<int> gmWeapons = GameController.gm.weapons;
        weapons = new int[gmWeapons.Count];
        for (int i = 0; i < gmWeapons.Count; i++) weapons[i] = gmWeapons[i];
        
        List<int> gmAuxGad = GameController.gm.auxGad;
        auxGad = new int[gmAuxGad.Count];
        for (int i = 0; i < gmAuxGad.Count; i++) auxGad[i] = gmAuxGad[i];
    }
}
