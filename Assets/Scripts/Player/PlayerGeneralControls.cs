using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGeneralControls : MonoBehaviour
{
    #region Parameters

    private float speedModifier;
    private float turnModifier;
    private Rigidbody2D _rigidbody2D;
    private float dirToRotate;
    
    [SerializeField] private GameObject wagonPrefab;
    private List<GameObject> wagons = new List<GameObject>();
    private List<Vector3> PositionsHistory = new List<Vector3>();
    private List<Quaternion> RotationsHistory = new List<Quaternion>();
    private List<Vector2> velocityHistory = new List<Vector2>();
    [SerializeField] private int Gap = 10;

    private GameController gm;
    
    #endregion

    private void Start()
    {
        gm = GameController.gm;
        
        speedModifier = PlayerStatus.status.GetMovimentSpeed();
        turnModifier = PlayerStatus.status.GetRotateModifier();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        
        InitializeShip();
    }

    private void Update()
    {
        dirToRotate = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        MoveFoward();
        
        UpdateRotation();
        
        PositionsHistory.Insert(0,transform.position);
        RotationsHistory.Insert(0,transform.rotation);
        velocityHistory.Insert(0,_rigidbody2D.velocity);

        int index = 1;
        foreach (var wagon in wagons)
        {
            Vector3 point = PositionsHistory[Mathf.Min(index * Gap, PositionsHistory.Count - 1)];
            Quaternion rotationTgt = RotationsHistory[Mathf.Min(index * Gap, PositionsHistory.Count - 1)];
            Vector2 velocityTgt = velocityHistory[Mathf.Min(index * Gap, velocityHistory.Count - 1)];
            wagon.transform.position = Vector3.Lerp(wagon.transform.position, point, 0.8f);
            wagon.transform.rotation = Quaternion.Lerp(wagon.transform.rotation, rotationTgt, 0.8f);
            //wagon.GetComponent<Rigidbody2D>().velocity = velocityTgt;
            
            
            index++;
        }
    }

    
    private void CreateWagon()
    {
        GameObject newWagon = Instantiate(wagonPrefab, transform.parent);
        wagons.Add(newWagon);
    }

    private void InitializeShip()
    {
        Instantiate(gm.weaponsPrefab[gm.weapons[0]], transform.GetChild(0));
        if(gm.auxGad[0] != 0)
        {
            Instantiate(gm.auxPrefab[gm.auxGad[0]], transform);
        }
        for (int i = 1; i <= gm.wagonCount; i++)
        {
            GameObject newWagon = Instantiate(wagonPrefab, transform.parent);
            wagons.Add(newWagon);
            Instantiate(gm.weaponsPrefab[gm.weapons[i]], newWagon.transform.GetChild(0));
            if(gm.auxGad[i] != 0)
            {
                Instantiate(gm.auxPrefab[gm.auxGad[i]], newWagon.transform);
            }
        }
    }
    
    #region Moviments

    protected void UpdateRotation()
    {
        dirToRotate *= -1;
        transform.Rotate(Vector3.forward,dirToRotate*turnModifier);
    }
    
    
    protected void MoveFoward()
    {
        float angle = transform.rotation.eulerAngles.z+90;
        Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad),0);
        _rigidbody2D.velocity = dir * speedModifier;
    }
    
    protected void Stay()
    {
        _rigidbody2D.velocity = Vector2.zero;
    }

    #endregion
    
    protected void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag($"Enemy"))
        {
            col.gameObject.GetComponent<LifeSystem>().TakeDamage(10);
        }
    }
    
    
    
}
