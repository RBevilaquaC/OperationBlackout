using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHangar : MonoBehaviour
{
    #region Parameters

    [SerializeField] private Transform tgt;
    [SerializeField] private Vector3 offset;
    private bool canFollow;

    #endregion

    private void Start()
    {
        Invoke(nameof(TriggerFollow),2f);
    }

    private void LateUpdate()
    {
        if(canFollow) transform.position = Vector3.Lerp(tgt.position+offset, transform.position, 0.6f*Time.deltaTime);
    }

    private void TriggerFollow()
    {
        GetComponent<Animator>().enabled = false;
        canFollow = true;

    }
}
