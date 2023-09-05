using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointDefenseBullet : MonoBehaviour
{
    public GameObject tgt;
    private Rigidbody2D rig;
    public float speedModifier;
    public float turnModifier = 1;

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(tgt.activeSelf)
        {
            MoveFoward();
            LookToTgt();
        }
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
    
    protected void MoveFoward()
    {
        float angle = transform.rotation.eulerAngles.z+90;
        Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad),0);
        rig.velocity = dir * speedModifier;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (CompareTag("EnemyMissiles"))
        {
            col.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
