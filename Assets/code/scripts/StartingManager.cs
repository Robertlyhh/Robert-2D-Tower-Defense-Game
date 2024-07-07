using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartingManager : MonoBehaviour
{

    public void LoadMainMenu()
    {
        StartCoroutine(LoadMainMenuAsync());
    }

    private IEnumerator LoadMainMenuAsync()
    {
        // Unload current scene if it's not the MainMenu
        yield return SceneManager.UnloadSceneAsync("Starting");

        // Load the MainMenu scene as a single scene (not additively)
        yield return SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
