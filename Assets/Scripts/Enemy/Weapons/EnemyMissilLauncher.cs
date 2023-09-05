using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissilLauncher : EnemyWeaponController
{
    protected override void RequestCommonShoot()
    {

        GameObject bullet = bulletPool.transform.GetChild(currentBullet).gameObject;
        currentBullet++;
        if (currentBullet >= bulletPool.transform.childCount) currentBullet = 0;

        float angle = gunPivot.rotation.eulerAngles.z + 90;
        Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
        bullet.transform.position = gunPivot.position + dir * 0.2f;

        bullet.SetActive(true);
        bullet.GetComponent<Rigidbody2D>().velocity = dir * speedBulletModifier;
        bullet.transform.rotation = gunPivot.rotation;
        bullet.GetComponent<EnemyMissil>().damageAmount = damageAmount;
        bullet.GetComponent<EnemyMissil>().ArmMissil();
    }

    protected override void FillPool()
    {
        bulletPool = new GameObject();
        bulletPool.name = "BulletPool_" + transform.parent.parent.gameObject.name + "_" + gameObject.name;
        for (int i = 0; i < poolSize; i++)
        {
            GameObject newBullet = Instantiate(bulletPrefab, bulletPool.transform);
            newBullet.SetActive(false);
            Radar.radar.enemyMissiles.Add(newBullet);
        }
    }
}
