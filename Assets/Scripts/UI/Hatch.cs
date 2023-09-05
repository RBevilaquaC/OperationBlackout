using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hatch : MonoBehaviour
{
    private bool isTrigged;
    private GameObject exitPanel;
    [SerializeField] private GameObject auxText;

    private void Start()
    {
        exitPanel = GameController.gm.exitPanel;
    }

    private void Update()
    {
        if (isTrigged && Input.GetKeyDown(KeyCode.E) && exitPanel.activeSelf)
        {
            GameController.gm.QuitGame();
        }
        else if (isTrigged && Input.GetKeyDown(KeyCode.E))
        {
            GameController.gm.exitPanel.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            exitPanel.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        isTrigged = true;
        auxText.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isTrigged = false;
        exitPanel.SetActive(false);
        auxText.SetActive(false);
    }
}
