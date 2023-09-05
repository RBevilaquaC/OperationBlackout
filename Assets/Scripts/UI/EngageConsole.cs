using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngageConsole : MonoBehaviour
{
    #region Parameters
    
    private MenuBuilder builder;
    private bool playerNear;
    [SerializeField] private GameObject auxText;

    #endregion

    private void Start()
    {
        builder = transform.parent.gameObject.GetComponent<MenuBuilder>();
    }

    private void Update()
    {
        if(playerNear && Input.GetKeyDown(KeyCode.E))
            GameController.gm.LoadScene("GameScene");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        playerNear = true;
        auxText.SetActive(true);
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        playerNear = false;
        auxText.SetActive(false);
    }
}
