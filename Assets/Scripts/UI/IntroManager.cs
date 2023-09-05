using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        Invoke(nameof(LoadHangar),4.2f);
    }

    private void LoadHangar()
    {
        SceneManager.LoadScene("Hangar");
    }
}
