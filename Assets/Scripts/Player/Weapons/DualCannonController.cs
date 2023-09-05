using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualCannonController : WeaponController
{
    private float sideGun = 1;
    
    protected override void Update()
    {
        if(!GameController.gm.victoryPanel.activeSelf)
        {
            LookToMouse();
            if (timerCDShoot <= 0 && Input.GetButton("Fire1"))
            {
                //RequestShoot(); or another function
                RequestDualGunShoot();
                GameController.gm.EnergyConsume(energyConsumption);
                timerCDShoot = cdShoot;
            }
            else if (timerCDShoot >= 0) timerCDShoot -= Time.deltaTime;
        }
    }
    
    protected void RequestDualGunShoot()
    {
        GameObject bullet = bulletPool.transform.GetChild(currentBullet).gameObject;
        currentBullet++;
        if (currentBullet >= bulletPool.transform.childCount) currentBullet = 0;
        
        float angle = gunPivot.rotation.eulerAngles.z+90;
        Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad),0);
        Vector3 dirSide = new Vector3(Mathf.Cos((angle-90) * Mathf.Deg2Rad), Mathf.Sin((angle-90) * Mathf.Deg2Rad),0);
        bullet.transform.position = gunPivot.position + dir*0.2f + dirSide * (0.1f * sideGun);
        sideGun *= -1;
        
        bullet.SetActive(true);
        bullet.GetComponent<Rigidbody2D>().velocity = dir * speedBulletModifier;
        bullet.GetComponent<Bullet>().damageAmount = damageAmount;
    }
}
