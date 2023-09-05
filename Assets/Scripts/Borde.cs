using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Borde : MonoBehaviour
{
    private bool inAbyss;
    private float count;

    private void FixedUpdate()
    {
        if (count > 0) count -= Time.fixedDeltaTime;
        else if(inAbyss)
        {
            count = 1;
            PlayerStatus.playerObj.GetComponent<PlayerLife>().TakeDamage(8);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player")) inAbyss = true;
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player")) inAbyss = false;
    }
}
