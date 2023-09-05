using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleText : MonoBehaviour
{
    private bool isTrigged;
    private GameObject creditPanel;
    [SerializeField] private GameObject auxText;

    private void Start()
    {
        creditPanel = GameController.gm.creditPanel;
    }

    private void Update()
    {
        if (isTrigged && Input.GetKeyDown(KeyCode.E))
        {
            creditPanel.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            creditPanel.SetActive(false);
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
        creditPanel.SetActive(false);
        auxText.SetActive(false);
    }
}
