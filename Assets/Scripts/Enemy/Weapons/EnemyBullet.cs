using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damageAmount;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerStatus.playerObj.GetComponent<LifeSystem>().TakeDamage(damageAmount);
            gameObject.SetActive(false);
        }
    }
}
