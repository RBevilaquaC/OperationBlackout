using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotAnim : MonoBehaviour
{
    #region Parameters

    private Animator anim;
    private Rigidbody2D rig;
    private SpriteRenderer sprite;

    private bool dir;

    #endregion

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(rig.velocity != Vector2.zero)
        {
            anim.SetBool($"Moving", true);
            if (rig.velocity.x > 0.2f) dir = false;
            else if (rig.velocity.x < -0.2f) dir = true;
            
            sprite.flipX = dir;
        }
        else anim.SetBool($"Moving", false);
    }
}
