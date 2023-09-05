using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    #region Parameters

    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected float turnModifier;
    [SerializeField] protected float speedBulletModifier;
    [SerializeField] protected float cdShoot;
    [SerializeField] protected float maxTrackingRange;
    [SerializeField] protected int poolSize;
    [SerializeField] protected int damageAmount;
    [SerializeField] protected int energyConsumption;
    protected Radar radar;
    protected Transform gunPivot;
    protected GameObject bulletPool;
    protected float timerCDShoot;
    protected int currentBullet;

    #endregion

    protected virtual void Start()
    {
        radar = Radar.radar;
        FillPool();
        gunPivot = transform.parent;
    }

    protected virtual void Update()
    {
        if(!GameController.gm.victoryPanel.activeSelf)
        {
            LookToMouse();
            if (timerCDShoot <= 0 && Input.GetButton("Fire1"))
            {
                //RequestShoot(); or another function
                RequestCommonShoot();
                GameController.gm.EnergyConsume(energyConsumption);
                timerCDShoot = cdShoot;
            }
            else if (timerCDShoot >= 0) timerCDShoot -= Time.deltaTime;
        }
    }

    protected virtual void FillPool()
    {
        bulletPool = new GameObject();
        bulletPool.name = "BulletPool_" + transform.parent.parent.gameObject.name + "_" +gameObject.name;
        for (int i = 0; i < poolSize; i++)
        {
            GameObject newBullet = Instantiate(bulletPrefab, bulletPool.transform);
            newBullet.SetActive(false);
        }
    }
    
    protected void LookToPlayer()
    {
        Vector3 playerPosition = PlayerStatus.playerObj.transform.position;
        Vector3 direction = playerPosition - gunPivot.position;
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        Vector3 targetRotation = new Vector3(0, 0, angle-90);
        gunPivot.rotation = (Quaternion.Lerp(gunPivot.rotation, Quaternion.Euler(targetRotation), turnModifier));
    }
    
    protected void LookToEnemyLowLife()
    {
        List<GameObject> enemies = radar.GetActiveEnemies();
        GameObject tgt = NearEnemy();
        if(tgt != null)
        {
            foreach (var enemy in enemies)
            {
                if (enemy.GetComponent<EnemyLife>().GetCurrentLife() < tgt.GetComponent<EnemyLife>().GetCurrentLife())
                {
                    if ((enemy.transform.position - transform.position).magnitude <= maxTrackingRange)
                    {
                        tgt = enemy;
                    }
                }
            }

            Vector3 tgtPosition = tgt.transform.position;
            Vector3 direction = tgtPosition - gunPivot.position;
            float angle = Vector2.SignedAngle(Vector2.right, direction);
            Vector3 targetRotation = new Vector3(0, 0, angle - 90);
            gunPivot.rotation = (Quaternion.Lerp(gunPivot.rotation, Quaternion.Euler(targetRotation), turnModifier));
        }
    }
    
    protected void LookToNearEnemy()
    {
        List<GameObject> enemies = radar.GetActiveEnemies();
        GameObject tgt = NearEnemy();
        if(tgt != null)
        {
            Vector3 tgtPosition = tgt.transform.position;
            Vector3 direction = tgtPosition - gunPivot.position;
            float angle = Vector2.SignedAngle(Vector2.right, direction);
            Vector3 targetRotation = new Vector3(0, 0, angle - 90);
            gunPivot.rotation = (Quaternion.Lerp(gunPivot.rotation, Quaternion.Euler(targetRotation), turnModifier));
        }
    }
    
    protected void LookToEnemyHighLife()
    {
        List<GameObject> enemies = radar.GetActiveEnemies();
        GameObject tgt = NearEnemy();
        if(tgt != null)
        {
            foreach (var enemy in enemies)
            {
                if (enemy.GetComponent<EnemyLife>().GetCurrentLife() > tgt.GetComponent<EnemyLife>().GetCurrentLife())
                {
                    if ((enemy.transform.position - transform.position).magnitude <= maxTrackingRange)
                    {
                        tgt = enemy;
                    }
                }
            }

            Vector3 tgtPosition = tgt.transform.position;
            Vector3 direction = tgtPosition - gunPivot.position;
            float angle = Vector2.SignedAngle(Vector2.right, direction);
            Vector3 targetRotation = new Vector3(0, 0, angle - 90);
            gunPivot.rotation = (Quaternion.Lerp(gunPivot.rotation, Quaternion.Euler(targetRotation), turnModifier));
        }
    }
    
    protected void LookToMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - gunPivot.position;
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        Vector3 targetRotation = new Vector3(0, 0, angle-90);
        gunPivot.rotation = (Quaternion.Lerp(gunPivot.rotation, Quaternion.Euler(targetRotation), turnModifier));
    }

    protected GameObject NearEnemy()
    {
        List<GameObject> enemies = radar.GetActiveEnemies();
        GameObject tgt = null;
        float dist = 999;
        foreach (var enemy in enemies)
        {
            float newDist = (enemy.transform.position - transform.position).magnitude;
            if (dist > newDist)
            {
                tgt = enemy;
                dist = newDist;

            }
        }
        return tgt;
    }
    
    
    protected virtual void RequestCommonShoot()
    {
        GameObject bullet = bulletPool.transform.GetChild(currentBullet).gameObject;
        currentBullet++;
        if (currentBullet >= bulletPool.transform.childCount) currentBullet = 0;
        
        float angle = gunPivot.rotation.eulerAngles.z+90;
        Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad),0);
        bullet.transform.position = gunPivot.position + dir*0.2f;
        
        bullet.SetActive(true);
        bullet.GetComponent<Rigidbody2D>().velocity = dir * speedBulletModifier;
        bullet.GetComponent<Bullet>().damageAmount = damageAmount;
    }
}
