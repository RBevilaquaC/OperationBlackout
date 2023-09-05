using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Radar : MonoBehaviour
{
    #region Parameters

    public static Radar radar;

    public List<GameObject> playerMissiles = new List<GameObject>();
    public List<GameObject> enemyMissiles = new List<GameObject>();
    
    [SerializeField] private EnemySpawnner spawnner;
    [SerializeField] private GameObject pingPrefab;
    private List<List<GameObject>> enemiesPool;
    private GameObject[] enemiesPing;
    private List<GameObject> bossesPool;
    private List<GameObject> bossesPing = new List<GameObject>();
    private List<GameObject> activeEnemies;
    private List<GameObject> activeBosses = new List<GameObject>();
    private bool canScan;
    
    [SerializeField] private Transform pointNE;
    [SerializeField] private Transform pointSW;
    
    #endregion

    
    private void Awake()
    {
        radar = this;
    }

    private void Start()
    {
        canScan = false;
    }
    
    private void Update()
    {
        if (canScan) UpdateRadar();
    }

    private void UpdateRadar()
    {
        /*
        foreach (var ping in enemiesPing)
        {
            ping.SetActive(false);
        }
        foreach (var ping in bossesPool)
        {
            ping.SetActive(false);
        }*/
        
        foreach (var enemy in activeEnemies)
        {
            GameObject currentPing = enemiesPing[activeEnemies.IndexOf(enemy)];
            currentPing.SetActive(false);
            if (enemy.activeSelf)
            {
                currentPing.SetActive(true);
                currentPing.transform.position =
                    ((enemy.transform.position - PlayerStatus.playerObj.transform.position)/20) + transform.position;
                Vector3 dir = currentPing.transform.position;
                if(pointNE.position.x < dir.x || pointNE.position.y < dir.y ||
                   pointSW.position.x > dir.x || pointSW.position.y > dir.y) currentPing.SetActive(false);
            }
            else
            {
                enemiesPing[activeEnemies.Count-1].SetActive(false);
                activeEnemies.Remove(enemy);
                GameController.gm.enemiesActiveCount = activeEnemies.Count;
                UpdateRadar();
                break;
            }
        }

        for(int i = 0; i < activeBosses.Count; i++)
        {
            GameObject currentPing = bossesPing[i];
            currentPing.SetActive(false);
            if (activeBosses[i].activeSelf)
            {
                currentPing.SetActive(true);
                currentPing.transform.position =
                    ((activeBosses[i].transform.position - PlayerStatus.playerObj.transform.position)/20) + transform.position;
                Vector3 dir = currentPing.transform.position;
                if(pointNE.position.x < dir.x || pointNE.position.y < dir.y ||
                   pointSW.position.x > dir.x || pointSW.position.y > dir.y) currentPing.SetActive(false);
            }
            else
            {
                bossesPing[activeBosses.Count-1].SetActive(false);
                activeBosses.Remove(activeBosses[i]);
                GameController.gm.enemiesActiveCount = activeBosses.Count;
                GameController.gm.bossesActiveCount = activeBosses.Count;
                UpdateRadar();
                break;
            }
            
        }

        foreach (var boss in activeBosses)
        {
            if (!boss.activeSelf) activeBosses.Remove(boss);
        }

        foreach (var enemy in activeEnemies)
        {
            if (!enemy.activeSelf) activeEnemies.Remove(enemy);
        }
        
        GameController.gm.CheckFinishTheStage();    
        
    }

    public void StartScan()
    {
        canScan = true;
        enemiesPool = spawnner.GetEnemiesObjs();
        bossesPool = spawnner.GetBossesObjs();
        FillPingPool();
        CheckActiveEnemies();
    }

    private void FillPingPool()
    {
        GameObject pingPool = new GameObject();
        pingPool.transform.position = transform.position;
        pingPool.transform.parent = transform;
        pingPool.name = "PingPool";

        enemiesPing = new GameObject[enemiesPool.Count * enemiesPool[0].Count];
        for (int i = 0; i < enemiesPing.Length; i++)
        {
            GameObject newPing = Instantiate(pingPrefab,pingPool.transform);
            newPing.SetActive(false);
            enemiesPing[i] = newPing;
        }
        
        for (int i = 0; i < bossesPool.Count; i++)
        {
            GameObject newPing = Instantiate(pingPrefab,pingPool.transform);
            newPing.GetComponent<Light2D>().pointLightOuterRadius *= 3;
            newPing.SetActive(false);
            bossesPing.Add(newPing);
        }
    }

    private void CheckActiveEnemies()
    {
        List<GameObject> checkList = new List<GameObject>();
        foreach (List<GameObject> enemyType in enemiesPool)
            foreach (GameObject enemy in enemyType)
                if(enemy.activeSelf) checkList.Add(enemy);
        activeEnemies = checkList;
        
        
        List<GameObject> checkBossList = new List<GameObject>();
        foreach (GameObject boss in bossesPool)
            if(boss.activeSelf) checkBossList.Add(boss);
        activeBosses = checkBossList;

        GameController.gm.bossesActiveCount = activeBosses.Count;
        GameController.gm.enemiesActiveCount = activeEnemies.Count;
        GameController.gm.stageStart = true;
    }

    public List<GameObject> GetActiveEnemies()
    {
        return activeEnemies;
    }

    public List<GameObject> GetActiveBosses()
    {
        return activeBosses;
    }
}
