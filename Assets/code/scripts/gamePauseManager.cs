using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamePauseManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Animator pauseMenuAnimator;

    public static bool isPaused = false;

    private void Start()
    {
        isPaused = false;
    }  

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenuAnimator.SetBool("isPaused", isPaused);
    }

    public void PauseGame()
    { 
        if(isPaused)
        {   
            Debug.Log("Game is already paused");
            return;
        }
        TogglePause();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        TogglePause();
        Debug.Log("Game resumed");
    }

}
