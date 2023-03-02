using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    // Text that displays the status of the game
    public Text status;
    
    // Tracks if the game is paused or not
    private bool Paused; 

    // Program starts in a paused state
    void Start()
    {
        PauseProgram();
    }

    // Running state
    void ResumeProgram()
    {
        status.text = "Running";
        Time.timeScale = 1.0f;
        Paused = false;
    }

    // Running state
    void PauseProgram()
    {
        status.text = "Paused";
        Time.timeScale = 0.0f;
        Paused = true;
    }
    
    void Update()
    {
        // Pressing the escape button toggles
        // The status of the game
        // (pause and unpause)
        if (Input.GetKeyDown("escape"))
        {
            if (Paused == true)
            {
                ResumeProgram();
            }
            else
            {
                PauseProgram();
            }
        }
    }
}
