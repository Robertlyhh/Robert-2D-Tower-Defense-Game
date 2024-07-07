using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinHp : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private int currencyOnDeath = 10;

    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip[] audioClips;

    private bool isDead = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(float damage, int bulletType)
    {
        maxHp -= damage;
        if(maxHp <= 0 && !isDead)
        {
            enemySpawner.onEnemyDeath.Invoke();
            Debug.Log("Goblin died");
            levelManager.main.AddCurrency(currencyOnDeath);
            Debug.Log("Currency added: " + currencyOnDeath);
            audioSource.PlayOneShot(audioClips[0]);
            isDead = true;
            Destroy(gameObject);
        }
        if(audioSource == null)
        {
            Debug.Log("Audio source is null");
        }
        audioSource.PlayOneShot(audioClips[bulletType]);

    }
}
