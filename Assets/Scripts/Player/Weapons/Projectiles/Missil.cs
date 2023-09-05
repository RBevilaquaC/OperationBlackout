using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missil : Bullet
{
    #region Parameters

    [SerializeField] private float propellantModSpeed;
    [SerializeField] private float turnModifier;
    private Rigidbody2D rig;
    private float speedModifier;
    private bool firstStage;
    private GameObject tgt;

    #endregion

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (firstStage)
        {
            if (!tgt.activeSelf) tgt = NearEnemy();
            if(tgt != null)LookToTgt();
            MoveFoward();
        }
    }

    public void ArmMissil()
    {
        firstStage = false;
        speedModifier = propellantModSpeed * rig.velocity.magnitude;
        tgt = null;
        Invoke(nameof(Firstage),1);
    }

    private void Firstage()
    {
        tgt = NearEnemy();
        firstStage = true;
    }
    
    protected void MoveFoward()
    {
        float angle = transform.rotation.eulerAngles.z+90;
        Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad),0);
        rig.velocity = dir * speedModifier;
    }
    
    protected void LookToTgt()
    {
        if(tgt != null)
        {
            Vector3 tgtPosition = tgt.transform.position;
            Vector3 direction = tgtPosition - transform.position;
            float angle = Vector2.SignedAngle(Vector2.right, direction);
            Vector3 targetRotation = new Vector3(0, 0, angle - 90);
            transform.rotation = (Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRotation), turnModifier));
        }
    }
    
    protected GameObject NearEnemy()
    {
        List<GameObject> enemies = Radar.radar.GetActiveEnemies();
        GameObject tgt = null;
        float dist = 999;
        foreach (var enemy in enemies)
        {
            if(enemy.activeSelf)
            {
                float newDist = (enemy.transform.position - transform.position).magnitude;
                if (dist > newDist)
                {
                    tgt = enemy;
                    dist = newDist;
                }
            }
        }

        List<GameObject> bosses = Radar.radar.GetActiveBosses();
        foreach (var boss in bosses)
        {
            if(boss.activeSelf)
            {
                float newDist = (boss.transform.position - transform.position).magnitude;
                if (dist > newDist)
                {
                    tgt = boss;
                    dist = newDist;
                }
            }
        }
        
        return tgt;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            col.GetComponent<LifeSystem>().TakeDamage(damageAmount);
            gameObject.SetActive(false);
        }
    }
}
