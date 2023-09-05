using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotMoviment : MonoBehaviour
{
    #region Parameters

    private Rigidbody2D rig;
    private float speedMod = 5;
    private float dirAxis;

    #endregion

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        dirAxis = Input.GetAxis("Horizontal");
        rig.velocity = Vector2.right * (dirAxis * speedMod);
    }
}
