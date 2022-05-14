using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMae : MonoBehaviour
{
   [SerializeField]
    private bool PauseGame = false;
    void Start()
    {
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame = !PauseGame;
            Pause();
          
        }
    }
    void Pause()
    {
        if (PauseGame) Time.timeScale = 0.0f;
        if (!PauseGame) Time.timeScale = 1.0f;
    }
}
