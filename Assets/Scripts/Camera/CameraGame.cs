using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGame : MonoBehaviour
{
    #region Parameters

    private Transform tgt;
    private Vector3 offset = Vector3.back*10;
    private bool canFollow;

    #endregion

    private void Start()
    {
        tgt = PlayerStatus.playerObj.transform;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(tgt.position+offset, transform.position, 0.6f*Time.deltaTime);
    }
}
