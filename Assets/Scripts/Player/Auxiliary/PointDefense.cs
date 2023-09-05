using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointDefense : WeaponController
{
    #region Parameters

    private List<GameObject> missiles = new List<GameObject>();
    private GameObject tgt;

    #endregion

    private void Start()
    {
        gunPivot = transform.GetChild(0);
        
        radar = Radar.radar;
        missiles = Radar.radar.enemyMissiles;
        FillPool();
        gunPivot = transform.GetChild(0);
    }
    
    protected override void Update()
    {
        if(!GameController.gm.victoryPanel.activeSelf && missiles.Count > 0)
        {
            LookToTgt();
            if (timerCDShoot <= 0 && tgt != null && DistToTgt() < 5)
            {
                //RequestShoot(); or another function
                RequestCommonShoot();
                timerCDShoot = cdShoot;
            }
            else if (timerCDShoot >= 0) timerCDShoot -= Time.deltaTime;
        }
    }

    private float DistToTgt()
    {
        return (tgt.transform.position - transform.position).magnitude;
    }

    protected void LookToTgt()
    {
        NearTgt();
        if(tgt != null)
        {
            Vector3 direction = tgt.transform.position - gunPivot.position;
            float angle = Vector2.SignedAngle(Vector2.right, direction);
            Vector3 targetRotation = new Vector3(0, 0, angle - 90);
            gunPivot.rotation = (Quaternion.Lerp(gunPivot.rotation, Quaternion.Euler(targetRotation), turnModifier));
        }
    }
    
    protected void NearTgt()
    {
        tgt = null;
        float dist = 9999999;
        if(missiles.Count > 0)
        {
            foreach (var missile in missiles)
            {
                if (missile.activeSelf)
                {
                    float newDist = (missile.transform.position - transform.position).magnitude;
                    if (dist > newDist)
                    {
                        tgt = missile;
                        dist = newDist;
                    }
                }
            }
        }
    }
    
    protected virtual void RequestCommonShoot()
    {
        GameObject bullet = bulletPool.transform.GetChild(currentBullet).gameObject;
        currentBullet++;
        if (currentBullet >= bulletPool.transform.childCount) currentBullet = 0;
        
        float angle = gunPivot.rotation.eulerAngles.z+90;
        Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad),0);
        bullet.transform.position = transform.position;
        
        bullet.SetActive(true);
        bullet.GetComponent<Rigidbody2D>().velocity = dir * speedBulletModifier;
        bullet.GetComponent<PointDefenseBullet>().tgt = tgt;
        bullet.GetComponent<PointDefenseBullet>().speedModifier = speedBulletModifier;
    }
}
