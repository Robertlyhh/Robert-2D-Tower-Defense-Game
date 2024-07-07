using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private GameObject towerObj;
    private Turret turret;
    private MageTower mageTower;
    private BombTower bombTower;
    private Color originalColor;
    public bool targetRangeVisible = false;
    private bool isMageTower = false;
    private bool isBombTower = false;


    private void Start()
    {
        originalColor = sr.color;
    }

    private void OnMouseEnter()
    {
        sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        sr.color = originalColor;
    }

    private void OnMouseDown()
    {
        if (gamePauseManager.isPaused)
        {
            return;
        }
        if (UIManager.main.GetHoveringUI())
        {
            return;
        }

        if(towerObj != null)
        {
            if(isMageTower)
            {
                mageTower.OpenUpgradeUI();
                targetRangeVisible = !targetRangeVisible;

                if(targetRangeVisible)
                {
                    mageTower.SetupLineRenderer();
                }else
                {
                    mageTower.lineRenderer.positionCount = 0;
                }   

            }else if(turret != null)
            {
                turret.OpenUpgradeUI();

                targetRangeVisible = !targetRangeVisible;

                if(targetRangeVisible)
                {
                    turret.SetupLineRenderer();
                }else
                {
                    turret.lineRenderer.positionCount = 0;
                }
            }else{
                bombTower.OpenUpgradeUI();

                targetRangeVisible = !targetRangeVisible;

                if(targetRangeVisible)
                {
                    bombTower.SetupLineRenderer();
                }else
                {
                    bombTower.lineRenderer.positionCount = 0;
                }
            }            
                return;
        }

        Tower building = buildingManager.main.GetSelectedBuilding();

        if(levelManager.main.currency < building.cost)
        {
            Debug.Log("Not enough currency");
            return;
        }

        levelManager.main.RemoveCurrency(building.cost);

        if(building == null)
        {
            return;
        }

        towerObj = Instantiate(building.prefab, transform.position, Quaternion.identity);
        turret = towerObj.GetComponent<Turret>();
        if(turret == null)
        {
            mageTower = towerObj.GetComponent<MageTower>();
            if(mageTower == null)
            {
                bombTower = towerObj.GetComponent<BombTower>();
                isBombTower = true;
            }else
            {
                isMageTower = true;
            }
        }

        Transform turret0Transform = towerObj.transform.Find("turret_0");
        if (turret0Transform != null)
        {
            SpriteRenderer towerSr = turret0Transform.GetComponent<SpriteRenderer>();
            if (towerSr != null)
            {
                //towerSr.sortingOrder = sr.sortingOrder + 1;
            }
        }
    }

    void ChangeSortingOrder(int newOrder)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = newOrder;
        }
    }
}
