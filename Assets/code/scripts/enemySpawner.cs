using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class enemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GameObject[] enemyPrefabs;

    [Header("Attributes")]
    [SerializeField] private int baseEnemys = 8;
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyMultiplier = 1.2f;
    [SerializeField] private background background;

    [Header("Events")]
    public static UnityEvent onEnemyDeath = new UnityEvent();
    public AudioClip StartWaveSound; // Assign the hover sound here
    private AudioSource audioSource;

    public int waveNumber = 0;
    private float timeSinceLastSpawn;
    private int enemysSpawned = 0;
    private int enemysToSpawn;
    private bool waveInProgress = false;
    private int enemysAlive = 0;
    public Button StartPointButton;
    public Button EndPointButton;
    private LevelsManager LevelsManager;

    private void Awake()
    {
        onEnemyDeath.AddListener(enemyDeath);
    }


    private void Start()
    {
        // Start the wave coroutine
        StartCoroutine(startWave());
    }

    private void Update()
    {
        if(!waveInProgress || levelManager.main.gameIsOver || gamePauseManager.isPaused)
        {
            return;
        }
        
        timeSinceLastSpawn += Time.deltaTime;

        if(timeSinceLastSpawn >= (spawnRate) && enemysSpawned < enemysToSpawn)
        {
            spawnEnemy();
            enemysSpawned++;
            enemysAlive++;
            timeSinceLastSpawn = 0;

        }

        if(enemysAlive == 0 && enemysSpawned == enemysToSpawn)
        {
            endWave();
        }

    }

    private IEnumerator startWave()
    {
        StartPointButton.gameObject.SetActive(true);
        EndPointButton.gameObject.SetActive(true);
        yield return new WaitForSeconds(timeBetweenWaves);

        waveInProgress = true;
        enemysToSpawn = enemiesPerWave();
        audioSource = GetComponent<AudioSource>();

        audioSource.PlayOneShot(StartWaveSound);
        StartPointButton.gameObject.SetActive(false);
        EndPointButton.gameObject.SetActive(false);
    }

    private void endWave()
    {
        waveInProgress = false;
        waveNumber++;
        enemysSpawned = 0;
        timeSinceLastSpawn = 0f;
        if(waveNumber == 5)
        {
            background.ShowGameWinningUI();
            levelManager.main.gameIsOver = true;
            CompleteLevel();
        }
        StartCoroutine(startWave());
    }

    private int enemiesPerWave()
    {
        return (int)(baseEnemys * Mathf.Pow(difficultyMultiplier, waveNumber));
    }

    private void spawnEnemy()
    {
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject prefabToSpawn = enemyPrefabs[randomIndex];
        Instantiate(prefabToSpawn, levelManager.main.startPoint.position, Quaternion.identity);
    }

    private void enemyDeath()
    {
        enemysAlive--;
    }

    public int currentLevel = 1;

    private void CompleteLevel()
    {
        LevelsManager levelsManager = FindObjectOfType<LevelsManager>();
        if (levelsManager != null)
        {
            levelsManager.SetHighestLevelWon(currentLevel);
        }
    }

}
