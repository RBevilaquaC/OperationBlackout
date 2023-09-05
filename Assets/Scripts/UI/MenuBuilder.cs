using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;

public class MenuBuilder : MonoBehaviour
{
    #region Parameters

    [SerializeField] private GameObject trainHead;
    [SerializeField] private GameObject wagon;
    [SerializeField] private GameObject[] weaponsSprites;
    [SerializeField] private GameObject[] auxSprites;
    [SerializeField] private GameObject lamp;
    
    private int wagonCount;
    private int[] weaponList;
    private int[] auxEquip;

    private GameObject lamps;
    private GameObject spaceTrain;
    private GameObject wagonsComponents;
    private GameObject weaponsComponents;
    private List<GameObject> weaponsComponentsObjs = new List<GameObject>();
    private GameObject auxiliaryComponents;
    private List<GameObject> auxiliaryComponentsObjs = new List<GameObject>();
    
    private Vector3 lampOffset = new Vector3(0,2.55f,0);
    private Vector3 interval = new Vector3(8.5f,0,0);
    private Vector3 wagonOffset = new Vector3(0,0f,0);
    private float TrainTgtPos;
    
    [SerializeField] private GameObject pilot;
    [SerializeField] private GameObject buildingMonitor;
    private TMP_Text buildingText;
    private bool consoleOpen;
    private bool selectWeapon;
    private bool selectAuxiliary;

    private string[] buildingMsgs = new string[3];
    private string[] weaponMsgs = new string[3];
    private string[] auxiliaryMsgs = new string[3];

    [SerializeField] private HangarConsole hConsole;

    #endregion

    private void Start()
    {
        lamps = new GameObject();
        lamps.transform.parent = transform;
        lamps.name = "Lamps";

        spaceTrain = new GameObject();
        spaceTrain.transform.parent = transform;
        spaceTrain.transform.position = Vector3.zero;
        spaceTrain.name = "SpaceTrain";

        UpdateParameters();

        CreateLamps();
        CreateShip();
        FillBuildingMsgs();
        TriggerConsoleBuilder(false);
    }
    
    private void Update()
    {
        if(spaceTrain.transform.position.x < TrainTgtPos - 0.5) spaceTrain.transform.position += Vector3.right * (Time.deltaTime * 60);
        else if(spaceTrain.transform.position.x > TrainTgtPos + 0.5) spaceTrain.transform.position -= Vector3.right * (Time.deltaTime * 60);

        if (consoleOpen)
        {
            ConsoleManager();
        }
    }

    private void BuildingState(bool state)
    {
        buildingMonitor.transform.GetChild(0).gameObject.SetActive(state);
        buildingMonitor.transform.GetChild(1).gameObject.SetActive(state);
    }
    
    private void ConsoleManager()
    {
        int trainIndex = (int)(TrainTgtPos / interval.x);
        
        if (Input.GetButtonDown("Horizontal"))
        {
            float dirToMove = Input.GetAxisRaw("Horizontal");
            TrainTgtPos += dirToMove * interval.x;
            if((wagonCount + 1)*interval.x < TrainTgtPos - 0.1 ) TrainTgtPos -= interval.x;
            else if(0 > TrainTgtPos) TrainTgtPos += interval.x;

            trainIndex = (int)(TrainTgtPos / interval.x);
            
            if (trainIndex > wagonCount)
            {
                SetMonitorMsg(0,0);
            }
            else if(buildingText.text == buildingMsgs[0])
            {
                SetMonitorMsg(0,1);
            }
        }

        if (Input.GetButtonDown("Vertical"))
        {
            if (trainIndex <= wagonCount && !selectWeapon && !selectAuxiliary)
            {
                if (buildingText.text == buildingMsgs[1])
                    buildingText.text = buildingMsgs[2];
                else if (buildingText.text == buildingMsgs[2])
                    buildingText.text = buildingMsgs[1];
            } 
            else if (selectWeapon)
            {
                int dir = (int)Input.GetAxisRaw("Vertical");
                int index = 0;
                for (int i = 0; i < weaponMsgs.Length; i++) if (weaponMsgs[i] == buildingText.text) index = i;
                
                index += dir;
                if (index < 0) index = weaponMsgs.Length - 1;
                else if (index >= weaponMsgs.Length) index = 0;
                
                if (weaponList[trainIndex] == index) index += dir;
                if (index < 0) index = weaponMsgs.Length - 1;
                else if (index >= weaponMsgs.Length) index = 0;
                
                SetMonitorMsg(1,index);
            }
            else if (selectAuxiliary)
            {
                int dir = (int)Input.GetAxisRaw("Vertical");
                int index = 0;
                for (int i = 0; i < auxiliaryMsgs.Length; i++) if (auxiliaryMsgs[i] == buildingText.text) index = i;
                index += dir;
                if (index < 0) index = auxiliaryMsgs.Length - 1;
                else if (index >= auxiliaryMsgs.Length) index = 0;
                
                if (auxEquip[trainIndex]-1 == index) index += dir;
                if (index < 0) index = auxiliaryMsgs.Length - 1;
                else if (index >= auxiliaryMsgs.Length) index = 0;
                
                SetMonitorMsg(2,index);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            trainIndex = (int)(TrainTgtPos / interval.x);
            
            if (buildingText.text == buildingMsgs[0])       //Create New Wagon - Cost == 25
            {
                if (GameController.gm.currentEnergy >= 25)
                {
                    GameController.gm.currentEnergy -= 25;
                    GameController.gm.wagonCount += 1;
                    GameController.gm.weapons.Add(0);
                    GameController.gm.auxGad.Add(0);

                    UpdateParameters();
                    
                    CreateNewWagon();

                    buildingText.text = buildingMsgs[1];
                }
            }
            else if (buildingText.text == buildingMsgs[1])  //Change Weapon
            {
                selectWeapon = true;
                SetMonitorMsg(1,1);
            }
            else if (buildingText.text == buildingMsgs[2])  //Change Auxiliary
            {
                selectAuxiliary = true;
                SetMonitorMsg(2,0);
            }
            
            
            else if (buildingText.text == weaponMsgs[0])    //Buy Cannon - Cost == 0
            {
                GameController.gm.weapons[trainIndex] = 0;
                
                GameObject auxObj = weaponsComponentsObjs[trainIndex];
                weaponsComponentsObjs[trainIndex] = Instantiate(weaponsSprites[GameController.gm.weapons[trainIndex]],auxObj.transform.position,auxObj.transform.rotation,auxObj.transform.parent);
                Destroy(auxObj);

                UpdateParameters();
            }
            else if (buildingText.text == weaponMsgs[1])    //Buy Dual Cannon - Cost == 25
            {
                if (GameController.gm.currentEnergy >= 25)
                {
                    GameController.gm.currentEnergy -= 25;
                    GameController.gm.weapons[trainIndex] = 1;
                    
                    GameObject auxObj = weaponsComponentsObjs[trainIndex];
                    weaponsComponentsObjs[trainIndex] = Instantiate(weaponsSprites[GameController.gm.weapons[trainIndex]],auxObj.transform.position,auxObj.transform.rotation,auxObj.transform.parent);
                    Destroy(auxObj);
                    
                    UpdateParameters();
                }
            }
            else if (buildingText.text == weaponMsgs[2])    //Buy Missile Launcher - Cost == 50
            {
                if (GameController.gm.currentEnergy >= 50)
                {
                    GameController.gm.currentEnergy -= 50;
                    GameController.gm.weapons[trainIndex] = 2;
                    
                    GameObject auxObj = weaponsComponentsObjs[trainIndex];
                    weaponsComponentsObjs[trainIndex] = Instantiate(weaponsSprites[GameController.gm.weapons[trainIndex]],auxObj.transform.position,auxObj.transform.rotation,auxObj.transform.parent);
                    Destroy(auxObj);
                    
                    UpdateParameters();
                }
                
            }
            
            
            else if (buildingText.text == auxiliaryMsgs[0]) //Buy Point Defense - Cost == 25
            {
                if (GameController.gm.currentEnergy >= 25)
                {
                    GameController.gm.currentEnergy -= 25;
                    GameController.gm.auxGad[trainIndex] = 1;
                    
                    GameObject auxObj = auxiliaryComponentsObjs[trainIndex];
                    auxiliaryComponentsObjs[trainIndex] = Instantiate(auxSprites[GameController.gm.auxGad[trainIndex]],auxObj.transform.position,auxObj.transform.rotation,auxObj.transform.parent);
                    Destroy(auxObj);
                    
                    UpdateParameters();
                }
            }
            else if (buildingText.text == auxiliaryMsgs[1]) //Buy Generator - Cost == 50
            {
                if (GameController.gm.currentEnergy >= 50)
                {
                    GameController.gm.currentEnergy -= 50;
                    GameController.gm.auxGad[trainIndex] = 2;
                    
                    GameObject auxObj = auxiliaryComponentsObjs[trainIndex];
                    auxiliaryComponentsObjs[trainIndex] = Instantiate(auxSprites[GameController.gm.auxGad[trainIndex]],auxObj.transform.position,auxObj.transform.rotation,auxObj.transform.parent);
                    Destroy(auxObj);

                    UpdateParameters();
                }
            }
            else if (buildingText.text == auxiliaryMsgs[2]) //Buy Battery - Cost == 25
            {
                if (GameController.gm.currentEnergy >= 25)
                {
                    GameController.gm.currentEnergy -= 25;
                    GameController.gm.auxGad[trainIndex] = 3;
                    
                    GameObject auxObj = auxiliaryComponentsObjs[trainIndex];
                    auxiliaryComponentsObjs[trainIndex] = Instantiate(auxSprites[GameController.gm.auxGad[trainIndex]],auxObj.transform.position,auxObj.transform.rotation,auxObj.transform.parent);
                    Destroy(auxObj);

                    UpdateParameters();
                }
                
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(!selectWeapon && !selectAuxiliary)
            {
                TriggerConsoleBuilder(false);
            }
            else if (selectWeapon)
            {
                selectWeapon = false;
                SetMonitorMsg(0,1);
            }
            else if (selectAuxiliary)
            {
                selectAuxiliary = false;
                SetMonitorMsg(0,2);
            }
        }
    }

    private void CreateNewWagon()
    {
        GameObject newWagon = Instantiate(wagon);
        newWagon.transform.position = wagonOffset;
        newWagon.transform.parent = wagonsComponents.transform;
        
        GameObject newWeapon = Instantiate(weaponsSprites[weaponList[wagonCount]]);
        newWeapon.transform.position = wagonOffset;
        newWeapon.transform.parent = weaponsComponents.transform;
        weaponsComponentsObjs.Add(newWeapon);
        
        GameObject newAux = new GameObject();
        newAux.transform.position = wagonOffset;
        newAux.transform.parent = auxiliaryComponents.transform;
        auxiliaryComponentsObjs.Add(newAux);
        
    }

    public void TriggerConsoleBuilder(bool state)
    {
        if(!state) hConsole.text.text = hConsole.msgs[0];
        pilot.GetComponent<PilotMoviment>().enabled = !state;
        pilot.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        consoleOpen = state;
        BuildingState(state);
    }

    private void SetMonitorMsg(int type, int msgId)
    {
        if(type == 0) buildingText.text = buildingMsgs[msgId];
        else if(type == 1) buildingText.text = weaponMsgs[msgId];
        else if(type == 2) buildingText.text = auxiliaryMsgs[msgId];
    }
    
    private void FillBuildingMsgs()
    {
        buildingText = buildingMonitor.transform.GetChild(1).GetChild(0).gameObject.GetComponent<TMP_Text>();
        
        buildingMsgs[0] = "Create New Wagon\nCost: 25";
        buildingMsgs[1] = "Change Weapon";
        buildingMsgs[2] = "Change Auxiliary";

        weaponMsgs[0] = "Buy Cannon\nCost: 0";
        weaponMsgs[1] = "Buy Dual Cannon\nCost: 25";
        weaponMsgs[2] = "Buy Missile Launcher\nCost: 50";

        auxiliaryMsgs[0] = "Buy Point Defense\nCost: 25";
        auxiliaryMsgs[1] = "Buy Generator\nCost: 50";
        auxiliaryMsgs[2] = "Buy Battery\nCost: 25";
        
        buildingText.text = buildingMsgs[1];
    }

    private void UpdateParameters()
    {
        wagonCount = GameController.gm.wagonCount;

        List<int> gmWeapons = GameController.gm.weapons;
        weaponList = new int[gmWeapons.Count];
        for (int i = 0; i < gmWeapons.Count; i++) weaponList[i] = gmWeapons[i];
        
        List<int> gmAuxGad = GameController.gm.auxGad;
        auxEquip = new int[gmAuxGad.Count];
        for (int i = 0; i < gmAuxGad.Count; i++) auxEquip[i] = gmAuxGad[i];
    }
    
    #region ScenarioRenderer
    
    private void CreateLamps()
    {
        Vector3 initialPoint = interval * -11;
        for (int i = 0; i <= 22; i++)
        {
            GameObject newLamp = Instantiate(lamp,lamps.transform);
            newLamp.transform.position = initialPoint + lampOffset + interval*i;
        }
    }
    
    private void CreateShip()
    {
        UpdateParameters();
        
        wagonsComponents = new GameObject();
        wagonsComponents.name = "wagonsComponents";
        wagonsComponents.transform.parent = spaceTrain.transform;
        
        weaponsComponents = new GameObject();
        weaponsComponents.name = "weaponsComponents";
        weaponsComponents.transform.parent = spaceTrain.transform;
        
        auxiliaryComponents = new GameObject();
        auxiliaryComponents.name = "auxiliaryComponents";
        auxiliaryComponents.transform.parent = spaceTrain.transform;
        
        GameObject head = Instantiate(trainHead, wagonsComponents.transform);
        head.transform.position = wagonOffset;
        GameObject weaponHead = Instantiate(weaponsSprites[weaponList[0]],weaponsComponents.transform);
        weaponHead.transform.position = wagonOffset;
        weaponsComponentsObjs.Add(weaponHead);
        if(auxEquip[0] != 0)
        {
            GameObject auxHead = Instantiate(auxSprites[auxEquip[0]], auxiliaryComponents.transform);
            auxHead.transform.position = wagonOffset;
            auxiliaryComponentsObjs.Add(auxHead);
        }
        else
        {
            GameObject auxHead = new GameObject();
            auxHead.transform.parent = auxiliaryComponents.transform;
            auxHead.transform.position = wagonOffset;
            auxiliaryComponentsObjs.Add(auxHead);
        }
        
        Vector3 currentPos = wagonOffset - interval; 
        for (int i = 1; i <= wagonCount; i++)
        {
            GameObject newWagon = Instantiate(wagon, wagonsComponents.transform);
            newWagon.transform.position = currentPos;
            GameObject newWeapon = Instantiate(weaponsSprites[weaponList[i]],weaponsComponents.transform);
            newWeapon.transform.position = currentPos;
            weaponsComponentsObjs.Add(newWeapon);
            if(auxEquip[i] != 0)
            {
                GameObject newAux = Instantiate(auxSprites[auxEquip[i]], auxiliaryComponents.transform);
                newAux.transform.position = currentPos;
                auxiliaryComponentsObjs.Add(newAux);
            }
            else
            {
                GameObject newAux = new GameObject();
                newAux.transform.parent = auxiliaryComponents.transform;
                newAux.transform.position = currentPos;
                auxiliaryComponentsObjs.Add(newAux);
            }
            
            currentPos -= interval;
        }
        spaceTrain.transform.position = Vector3.left*100;
    }
    
    #endregion
}