using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corsair : EnemyBasicMoviments
{
    private void FixedUpdate()
    {
        lookToPlayer();
        MoveToSide();
    }
}
