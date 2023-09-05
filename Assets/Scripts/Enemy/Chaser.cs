using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : EnemyBasicMoviments
{
    private void Update()
    {
        MoveFoward();
        lookToNearWagon();
    }
}
