using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawnner : MonoBehaviour
{
    #region MyRegion

    [SerializeField] private Radar radar;
    [SerializeField] private int enemiesPoolSize;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private GameObject[] bosses;
    private Transform enemiesParent;
    private Transform bossesParent;
    private List<List<GameObject>> enemiesObjs = new List<List<GameObject>>();
    private List<GameObject> bossesObjs = new List<GameObject>();
    private int stageCount;

    #endregion

    private void Start()
    {
        enemiesParent = new GameObject().transform;
        enemiesParent.gameObject.name = "EnemiesPool";
        enemiesParent.parent = transform;

        stageCount = GameController.gm.stageCount;
        
        FillEnemiesPool();
        
        SpawnEnemies();
        
        radar.StartScan();
    }

    public List<List<GameObject>> GetEnemiesObjs()
    {
        return enemiesObjs;
    }

    public List<GameObject> GetBossesObjs()
    {
        return bossesObjs;
    }

    private void FillEnemiesPool()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            Transform enemyParent = new GameObject().transform;
            enemyParent.gameObject.name = enemies[i].name + "Pool";
            enemyParent.parent = enemiesParent;
            
            List<GameObject> newEnemies = new List<GameObject>();
            for (int j = 0; j < enemiesPoolSize; j++)
            {
                GameObject newEnemy = Instantiate(enemies[i], enemyParent);
                newEnemy.SetActive(false);
                newEnemies.Add(newEnemy);
            }
            enemiesObjs.Add(newEnemies);
        }

        foreach (var boss in bosses)
        {
            Transform bossesParent = new GameObject().transform;
            bossesParent.gameObject.name = "BossesPool";
            bossesParent.parent = transform;
            
            GameObject newBoss = Instantiate(boss, bossesParent);
            newBoss.SetActive(false);
            bossesObjs.Add(newBoss);
        }
    }
    
    private void SpawnEnemies()
    {
        for (int i = 0; i < stageCount && i < enemies.Length; i++)
        {
            // ReSharper disable once PossibleLossOfFraction
            for (int j = 0; j < Mathf.FloorToInt(stageCount * 3 / (i* 3 + 1)) && j < enemiesPoolSize; j++)
            {
                Vector3 newPos = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), 0).normalized * Random.Range(17,25);
                GameObject enemy = enemiesParent.GetChild(i).GetChild(j).gameObject;
                enemy.SetActive(true);
                enemy.GetComponent<EnemyLife>().Respawn();
                enemy.transform.position = newPos;
            }
        }

        if (stageCount % 5 == 0)
        {
            GameObject bossStage = bossesObjs[Random.Range(0,bossesObjs.Count-1)];
            Vector3 newPos = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), 0).normalized * Random.Range(17,25);
            bossStage.transform.position = newPos;
            bossStage.GetComponent<EnemyLife>().Respawn();
            bossStage.SetActive(true);
        }
    }
}
