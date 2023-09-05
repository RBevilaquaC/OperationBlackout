using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissil : EnemyBullet
{
    #region Parameters
    
    [SerializeField] private float propellantModSpeed;
    [SerializeField] private float turnModifier;
    private Rigidbody2D rig;
    private float speedModifier;
    private bool secondStage;
    private GameObject tgt;

    #endregion

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (secondStage)
        {
            MoveFoward();
            if (!tgt.activeSelf) tgt = NearPlayerWagon();
            LookToTgt();
        }
    }

    public void ArmMissil()
    {
        secondStage = false;
        speedModifier = propellantModSpeed * rig.velocity.magnitude;
        tgt = null;
        Invoke(nameof(UsePropellant),1f);
    }

    private void UsePropellant()
    {
        tgt = NearPlayerWagon();
        secondStage = true;
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
    
    protected GameObject NearPlayerWagon()
    {
        tgt = PlayerStatus.playerObj;
        float nearDist = (tgt.transform.position - transform.position).magnitude;
        for (int i = 1; i < PlayerStatus.playerObj.transform.parent.childCount; i++)
        {
            Transform currentWagon = PlayerStatus.playerObj.transform.parent.GetChild(i);
            if ((currentWagon.position - transform.position).magnitude < nearDist)
            {
                nearDist = (currentWagon.position - transform.position).magnitude;
                tgt = currentWagon.gameObject;
            }
        }
        return tgt;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerStatus.playerObj.GetComponent<LifeSystem>().TakeDamage(damageAmount);
            gameObject.SetActive(false);
        }

        if (col.CompareTag($"Bullet"))
        {
            col.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
