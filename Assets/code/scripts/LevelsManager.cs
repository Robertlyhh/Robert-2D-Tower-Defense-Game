using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class LevelsManager : MonoBehaviour
{
    private string currentSceneName = "MainMenu"; // Initial scene
    private const string HighestLevelKey = "HighestLevel";
    private TextMeshProUGUI levelLockedText;
    private GameObject mainMenuUI;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        mainMenuUI = GameObject.Find("MainMenuUI");
        if (mainMenuUI != null)
        {
            // Find the "Level Locked" text child object
            levelLockedText = mainMenuUI.transform.Find("LevelLocked").GetComponent<TextMeshProUGUI>();
        }
    }

    public int GetHighestLevelWon()
    {
        return PlayerPrefs.GetInt(HighestLevelKey, 0);
    }

    // Method to set the highest level won
    public void SetHighestLevelWon(int level)
    {
        int currentHighest = GetHighestLevelWon();
        if (level > currentHighest)
        {
            PlayerPrefs.SetInt(HighestLevelKey, level);
            PlayerPrefs.Save();
        }
    }

    public void ResetHighestLevelWon()
    {
        PlayerPrefs.SetInt(HighestLevelKey, 0);
        PlayerPrefs.Save();
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex > GetHighestLevelWon() + 1)
        {
            StartCoroutine(ShowLevelLockedText());
            return;
        }
        if (levelIndex == 0)
        {
            StartCoroutine(LoadLevelAsync("Starting"));
            return;
        }
        StartCoroutine(LoadLevelAsync("level" + levelIndex.ToString()));
    }

    private IEnumerator LoadLevelAsync(string levelName)
    {
        // Check if the current scene is "MainMenu" and it is loaded
        if (currentSceneName == "MainMenu" && SceneManager.GetSceneByName("MainMenu").isLoaded)
        {
            // Unload the "MainMenu" scene
            Debug.Log("Unloading MainMenu scene");
            yield return SceneManager.UnloadSceneAsync(currentSceneName);
        }

        // Load the new scene additively
        yield return SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);

        // Set the new scene as active
        Scene newScene = SceneManager.GetSceneByName(levelName);
        SceneManager.SetActiveScene(newScene);

        // Update the current scene name
        currentSceneName = levelName;
        if (currentSceneName != "MainMenu")
        {
            yield return SceneManager.UnloadSceneAsync("MainMenu");
        }
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadMainMenuAsync());
    }

    private IEnumerator LoadMainMenuAsync()
    {
        // Unload current scene if it's not the MainMenu
        yield return SceneManager.UnloadSceneAsync(currentSceneName);

        // Load the MainMenu scene as a single scene (not additively)
        yield return SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);

        // Update the current scene name
        currentSceneName = "MainMenu";
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator ShowLevelLockedText()
    {
        levelLockedText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        levelLockedText.gameObject.SetActive(false);
    }

}
