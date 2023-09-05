using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HangarConsole : MonoBehaviour
{
    #region Parameters
    
    [SerializeField] private GameObject auxText;
    public TMP_Text text;
    private MenuBuilder builder;
    private bool playerNear;
    public string[] msgs = new string[2];

    #endregion

    private void Start()
    {
        msgs[0] = "press 'E'";
        msgs[1] = "Press 'E' to select\nPress 'Q' to back\nAWSD to navigate";
        
        builder = transform.parent.gameObject.GetComponent<MenuBuilder>();
    }

    private void Update()
    {
        if(playerNear && Input.GetKeyDown(KeyCode.E))
        {
            builder.TriggerConsoleBuilder(true);
            text.text = msgs[1];
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        playerNear = true;
        auxText.SetActive(true);
    }
    
    private void OnTriggerStay2D(Collider2D col)
    {
        playerNear = true;
        auxText.SetActive(true);
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        playerNear = false;
        text.text = msgs[0];
        auxText.SetActive(false);
    }
}
