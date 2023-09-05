using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour
{
    #region Parameters

    public static GameObject playerObj;
    public static PlayerStatus status;
    private Animator anim;

    [HideInInspector] public bool isAttacking;
    
    [Header("Moviment Settings")] 
    [SerializeField] private float movimentSpeed;
    [SerializeField] private float rotateModifier;
    
    [Header("Life Settings")] 
    [SerializeField] private int maxLife;
    
    #endregion

    private void Awake()
    {
        playerObj = gameObject;
        status = this;
    }

    public float GetMovimentSpeed()
    {
        return movimentSpeed;
    }

    public float GetRotateModifier()
    {
        return rotateModifier;
    }

    public int GetMaxLife()
    {
        return maxLife;
    }
    
    public void PauseGame()
    {
        Time.timeScale = 0;
        
    }
    
    public void Resume()
    {
        Time.timeScale = 1;
        
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
