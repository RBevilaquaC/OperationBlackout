using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    #region Parameters

    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected float turnModifier;
    [SerializeField] protected float speedBulletModifier;
    [SerializeField] protected float cdShoot;
    [SerializeField] protected int poolSize;
    [SerializeField] protected int damageAmount;
    protected Transform gunPivot;
    protected GameObject bulletPool;
    protected float timerCDShoot;
    protected int currentBullet;

    #endregion

    protected virtual void Start()
    {
        FillPool();
        gunPivot = transform.parent;
    }

    protected virtual void Update()
    {
        if(!GameController.gm.victoryPanel.activeSelf)
        {
            LookToPlayer();
            if (timerCDShoot <= 0)
            {
                timerCDShoot = cdShoot;
                RequestCommonShoot();
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
        bullet.GetComponent<EnemyBullet>().damageAmount = damageAmount;
    }
}
