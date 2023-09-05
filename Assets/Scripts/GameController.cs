using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    #region Parameters
    
    public static GameController gm;
    
    public int stageCount;
    [Header("Player Settings")]
    public List<int> weapons;
    public GameObject[] weaponsPrefab;
    public List<int> auxGad;
    public GameObject[] auxPrefab;
    public int wagonCount;
    public int currentEnergy = 30;
    public int maxEnergy;
    public int defaultDegen = 0;
    
    [Header("Stage Settings")]
    public int enemiesActiveCount;
    public int bossesActiveCount;
    public bool stageStart;
    public string currentScene;
    
    [Header("UI Settings")]
    private Canvas canvas;
    public GameObject victoryPanel;
    public TMP_Text victoryText;
    public GameObject defeatPanel;
    public GameObject HUD;
    public Slider lifebar;
    public Slider powerbar;
    public TMP_Text lifeValue;
    public TMP_Text energyValue;
    private float countSeg = 1;
    private int generatorCount;
    public GameObject exitPanel;
    public GameObject creditPanel;
    [SerializeField] private GameObject saveFeedback;

    [Header("Gadget MOD")]
    [SerializeField] private int batteryMod;
    [SerializeField] private int generatorMod;
    
    #endregion

    private void Awake()
    {
        if (GameController.gm == null)
        {
            gm = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        if(File.Exists(SaveSystem.path)) LoadGame();
        
        canvas = transform.GetChild(0).gameObject.GetComponent<Canvas>();
        victoryPanel.SetActive(false);
        maxEnergy = 100;
        
        SceneManager.sceneLoaded += this.OnLoadCallback;
    }
    
    private void OnLoadCallback(Scene scene, LoadSceneMode sceneMode)
    {
        ResumeGame();
        UpdateMaxEnergy();
        CountGenerators();
        
        if(victoryPanel != null) victoryPanel.SetActive(false);
        if(defeatPanel != null) defeatPanel.SetActive(false);

        if(HUD != null) HUD.SetActive(scene.name == "GameScene");
        UpdateEnergyBar();

        print("newSceneIsloaded");
        currentScene = scene.name;
        
        if(scene.name != "Logo") SaveGame();
        if (weapons.Count == 0) RestartToInitialValues();

        UpdateCanvasParameters();
    }

    private void FixedUpdate()
    {
        if(stageStart) DegenEnergy();
    }

    private void DegenEnergy()
    {
        if (countSeg > 0) countSeg -= Time.fixedDeltaTime;
        else
        {
            currentEnergy+= (generatorCount * generatorMod)-defaultDegen;
            countSeg = 1;
            //if(currentEnergy == 0) Defeat();
            if (currentEnergy >= maxEnergy)
            {
                PlayerStatus.playerObj.GetComponent<PlayerLife>().Heal(currentEnergy % maxEnergy);
                currentEnergy = maxEnergy;
            }
            
            UpdateEnergyBar();
        }
    }

    private void CountGenerators()
    {
        generatorCount = 0;
        foreach (var aux in auxGad)
        {
            if (aux == 2) generatorCount++;
        }
    }

    private void UpdateMaxEnergy()
    {
        maxEnergy = 100;
        foreach (var aux in auxGad)
        {
            if (aux == 3) maxEnergy+=batteryMod;
        }
    }

    public void CheckFinishTheStage()
    {
        if (stageStart && enemiesActiveCount == 0 && bossesActiveCount == 0)
        {
            stageStart = false;
            victoryPanel.SetActive(true);
            victoryText.text = "Stage " + stageCount + "\n \nClear";
            stageCount++;
            PlayerStatus.playerObj.GetComponent<PlayerGeneralControls>().enabled = false;
            PlayerStatus.playerObj.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            PlayerStatus.playerObj.GetComponent<Rigidbody2D>().freezeRotation = true;
            PauseGame();
        }
    }

    public void Defeat()
    {
        stageStart = false;
        defeatPanel.SetActive(true);
        File.Delete(SaveSystem.path);
        RestartToInitialValues();
        SaveGame();
    }
    
    private void UpdateCanvasParameters()
    {
        if(canvas!= null)
        {
            canvas.worldCamera = Camera.main;
            canvas.sortingLayerName = "UI";
            canvas.sortingOrder = 15;
        }
    }

    private void UpdateEnergyBar()
    {
        powerbar.maxValue = maxEnergy;
        powerbar.value = currentEnergy;
        // ReSharper disable once HeapView.BoxingAllocation
        energyValue.text = $"{currentEnergy}/{maxEnergy}";
    }

    public void AddEnergy(int energyAmount)
    {
        currentEnergy += energyAmount;
        if (currentEnergy > maxEnergy)
        {
            PlayerStatus.playerObj.GetComponent<PlayerLife>().Heal(currentEnergy - maxEnergy);
            currentEnergy = maxEnergy;
        }
        UpdateEnergyBar();
    }

    public void EnergyConsume(int energyAmount)
    {
        currentEnergy -= energyAmount;
        //if (currentEnergy <= 0) Defeat();
        UpdateEnergyBar();
    }
    private void RestartToInitialValues()
    {
        auxGad = new List<int>();
        auxGad.Add(0);
        weapons = new List<int>();
        weapons.Add(0);
        stageCount = 1;
        wagonCount = 0;
        currentEnergy = 30;
        maxEnergy = 100;
    }

    #region UiRenderer


    #endregion

    #region sceneManager
    
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    
    public void ResumeGame()
    {
        Time.timeScale = 1;
        
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion

    #region SaveSystem

    public void SaveGame()
    {
        SaveSystem.SaveGame();
        saveFeedback.SetActive(true);
        Invoke("DisableSaveFeedbackState",3f);
    }

    private void DisableSaveFeedbackState()
    {
        saveFeedback.SetActive(false);
    }
    
    public void LoadGame()
    {
        GameData data = SaveSystem.LoadGame();
        currentEnergy = data.currentEnergy;
        wagonCount = data.wagons;
        stageCount = data.stageCount;
        
        weapons = new List<int>();
        foreach (var weapon in data.weapons) weapons.Add(weapon);
        
        auxGad = new List<int>();
        foreach (var aux in data.auxGad) auxGad.Add(aux);
    }

    #endregion
    
}
