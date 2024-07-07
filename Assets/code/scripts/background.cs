using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background : MonoBehaviour
{
    [SerializeField] private GameObject gameWinningUI;
    [SerializeField] private AudioClip winningMusic;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (levelManager.main.gameIsOver)
        {
            audioSource.Stop();
        }
        else
        {
            if(gamePauseManager.isPaused)
            {
                audioSource.Pause();
            }
            else
            {
                audioSource.UnPause();
            }
        }
    }

    public void ShowGameWinningUI()
    {
        StartCoroutine(WaitAndShowGameWinningUI());
    }

    public IEnumerator WaitAndShowGameWinningUI()
    {
        yield return new WaitForSeconds(3f);
        gameWinningUI.SetActive(true);
        AudioSource.PlayClipAtPoint(winningMusic, Camera.main.transform.position);
    }
}
