using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI currencyUI;
    [SerializeField] Animator anim;
    [SerializeField] TextMeshProUGUI lifeUI;
    [SerializeField] TextMeshProUGUI waveUI;

    private bool isMenuOpen = true;
    private enemySpawner spawner;

    private void Start()
    {
        spawner = GameObject.FindWithTag("GameController").GetComponent<enemySpawner>();
    }

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        anim.SetBool("MenuOpen", isMenuOpen);
    }

    private void OnGUI()
    {
        currencyUI.text = levelManager.main.currency.ToString();
        lifeUI.text = "Life: " + levelManager.main.baseLife.ToString();
        waveUI.text = "Wave: " + spawner.waveNumber.ToString() + "/5";
    }

    public void SetSelectedTower(int towerIndex)
    {
        buildingManager.main.SetSelectedTower(towerIndex);
    }
}
