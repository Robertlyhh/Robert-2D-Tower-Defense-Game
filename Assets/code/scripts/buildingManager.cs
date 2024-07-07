using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildingManager : MonoBehaviour
{
    public static buildingManager main;

    [Header("References")]
    //[SerializeField] private GameObject[] buildingPrefabs;
    [SerializeField] private Tower[] towers;

    private int selectedBuilding = 0;

    private void Awake()
    {
        if (main == null)
        {
            main = this;
            //DontDestroyOnLoad(gameObject);
        }
        else if (main != this)
        {
            Destroy(gameObject); 
        }
    }

    public Tower GetSelectedBuilding()
    {
        return towers[selectedBuilding];
    }

    public void SetSelectedTower(int _selectedTower)
    {
        selectedBuilding = _selectedTower;
    }
}
