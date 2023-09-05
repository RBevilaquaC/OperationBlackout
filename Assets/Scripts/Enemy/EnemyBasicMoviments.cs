using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasicMoviments : MonoBehaviour
{
    #region Parameters

    [SerializeField] protected float speedModifier;
    [SerializeField] protected float turnModifier;
    private Rigidbody2D _rigidbody2D;

    #endregion

    protected void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    protected float DirToPlayer()
    {
        return (PlayerStatus.playerObj.transform.position - transform.position).magnitude;
    }

    private GameObject NearestWagon()
    {
        GameObject nearestPlayer = PlayerStatus.playerObj;
        float nearDist = (nearestPlayer.transform.position - transform.position).magnitude;
        for (int i = 1; i < PlayerStatus.playerObj.transform.parent.childCount; i++)
        {
            Transform currentWagon = PlayerStatus.playerObj.transform.parent.GetChild(i);
            if ((currentWagon.position - transform.position).magnitude < nearDist)
            {
                nearDist = (currentWagon.position - transform.position).magnitude;
                nearestPlayer = currentWagon.gameObject;
            }
        }
        return nearestPlayer;
    }
    
    protected virtual void lookToPlayer()
    {
        Vector3 mousePosition = PlayerStatus.playerObj.transform.position;
        Vector3 direction = mousePosition - transform.position;
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        Vector3 targetRotation = new Vector3(0, 0, angle-90);
        transform.rotation = (Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRotation), turnModifier));
    }
    
    protected void lookToNearWagon()
    {
        Vector3 mousePosition = NearestWagon().transform.position;
        Vector3 direction = mousePosition - transform.position;
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        Vector3 targetRotation = new Vector3(0, 0, angle-90);
        transform.rotation = (Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRotation), turnModifier));
    }
    
    
    protected void MoveFoward()
    {
        float angle = transform.rotation.eulerAngles.z+90;
        Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad),0);
        _rigidbody2D.velocity = dir * speedModifier;
    }
    
    protected void MoveToSide()
    {
        float angle = transform.rotation.eulerAngles.z;
        Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad),0);
        _rigidbody2D.velocity = dir * speedModifier;
    }
    
    protected void Stay()
    {
        _rigidbody2D.velocity = Vector2.zero;
    }
    
    protected void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerStatus.playerObj.GetComponent<PlayerLife>().TakeDamage(25);
        }
    }

    protected void MoveBack()
    {
        float angle = transform.rotation.eulerAngles.z-90;
        Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad),0);
        _rigidbody2D.velocity = dir * speedModifier;
        
    }
}
