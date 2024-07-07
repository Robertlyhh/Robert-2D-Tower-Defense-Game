using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class levelManager : MonoBehaviour
{
    public static levelManager main;

    public Transform startPoint;
    public Transform[] path;

    public int currency = 1000;
    public int baseLife = 20;
    public bool gameIsOver = false;
    private string currentSceneName;

    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gamePauseUI;

    private void Awake()
    {
        main = this;
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    private void Start()
    {
        currency = 1000;
    }

    public void AddCurrency(int amount)
    {
        currency += amount;
        Debug.Log("Currency added: " + amount + ". New total: " + currency);
    }

    public void RemoveCurrency(int amount)
    {
        if(currency - amount < 0)
        {
            Debug.Log("Not enough currency");
            return;
        }
        else
        {
            currency -= amount;
            Debug.Log("Currency removed: " + amount + ". New total: " + currency);
        }
    }

    public void RemoveLife(int amount)
    {
        baseLife -= amount;
        Debug.Log("Life removed: " + amount + ". New total: " + baseLife);
        if(baseLife <= 0)
        {
            ShowGameOverUI();
            gameIsOver = true;
        }
    }

    private void ShowGameOverUI()
    {
        Time.timeScale = 0;
        gameOverUI.SetActive(true);
    }

    public void ResetGame()
    {
        gameOverUI.SetActive(false);
        //StartCoroutine(LoadLevelAsync("level1"));
        ReloadCurrentScene();
        Time.timeScale = 1;
    }


    private IEnumerator LoadLevelAsync(string levelName)
    {
        yield return SceneManager.UnloadSceneAsync(currentSceneName);

        // Load the new scene additively
        yield return SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);

        // Set the new scene as active
        Scene newScene = SceneManager.GetSceneByName(levelName);
        SceneManager.SetActiveScene(newScene);
    }

    public void ReloadCurrentScene()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }   


    public void GoMainMenu()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadMainMenuAsync());
    }

    private IEnumerator LoadMainMenuAsync()
    {
        // Unload current scene if it's not the MainMenu
        if (!string.IsNullOrEmpty(currentSceneName) && SceneManager.GetSceneByName(currentSceneName).isLoaded)
        {
            // 卸载当前场景
            yield return SceneManager.UnloadSceneAsync(currentSceneName);
        }

        // Load the MainMenu scene as a single scene (not additively)
        yield return SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
    }
    
    public void OnButtonClick(Button clickedButton)
    {
        clickedButton.gameObject.SetActive(false);
    }

}
