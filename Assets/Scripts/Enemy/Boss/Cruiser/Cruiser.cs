using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cruiser : EnemyBasicMoviments
{
    private void Update()
    {
        MoveFoward();
        lookToPlayer();
    }
    
    protected virtual void lookToPlayer()
    {
        Vector3 mousePosition = PlayerStatus.playerObj.transform.position;
        Vector3 direction = mousePosition - transform.position;
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        Vector3 targetRotation = new Vector3(0, 0, angle);
        transform.rotation = (Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRotation), turnModifier));
    }
    
}
