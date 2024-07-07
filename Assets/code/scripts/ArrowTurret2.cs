using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ArrowTurret2 : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private GameObject upgradeButton;
    [SerializeField] private GameObject sellButton;


    [Header("Attributes")]
    [SerializeField] private float targetRange = 3f;
    [SerializeField] private float fireRate = 2f;
    [SerializeField] private int BaseUpgradeCost = 100;
    [SerializeField] private int BaseSellPrice = 50;


    private Transform target;
    private float fireTimer;
    private int level = 2;
    private float targetRangeBase;
    private GameObject towerObj;

    private void Start()
    {
        targetRangeBase = targetRange;
        upgradeButton.GetComponent<Button>().onClick.AddListener(Upgrade);
        sellButton.GetComponent<Button>().onClick.AddListener(Sell);
    }

    private void Update()
    {   
        if(gamePauseManager.isPaused)
        {
            return;
        }
        
        if(target == null)
        {
            findTarget();
            return;
        }

        if(CheckTargetIsInRange())
        {
            target = null;
        }else
        {
            fireTimer -= Time.deltaTime;
            if(fireTimer <= 0)
            {
                fireTimer = fireRate;
                Fire();
            }
        }


    }

    private void findTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetRange, (Vector2)
        transform.position, 0, enemyMask);

        if(hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(transform.position, target.position) > targetRange;
    }

    private void Fire()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("bulletPrefab is not assigned.");
            return;
        }

        if (firingPoint == null)
        {
            Debug.LogError("firingPoint is not assigned.");
            return;
        }


        GameObject arrowObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        if (arrowObj != null)
        {
            arrow arrowScript = arrowObj.GetComponent<arrow>();
            if (arrowScript != null)
            {
                arrowScript.SetTarget(target);
            }
            else
            {
                Debug.LogError("Failed to get arrow component.");
            }
        }
        else
        {
            Debug.LogError("Failed to instantiate bulletPrefab.");
        }
    }

    public void OpenUpgradeUI()
    {
        upgradeUI.SetActive(true);
    }

    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
        UIManager.main.SetHoveringUI(false);
    }

    private void Upgrade()
    {
        if(levelManager.main.currency < GetUpgradeCost())
        {
            Debug.Log("Not enough currency");
            return;
        }

        levelManager.main.RemoveCurrency(GetUpgradeCost());
        //ReplaceTower();

        CloseUpgradeUI();
    }


    private int GetUpgradeCost()
    {
        return Mathf.RoundToInt(BaseUpgradeCost * Mathf.Pow(level, 0.8f));
    }

    private int GetTargetRange()
    {
        return Mathf.RoundToInt(targetRangeBase * Mathf.Pow(level, 0.6f));
    }

    private void Sell()
    {
        levelManager.main.AddCurrency(GetSellPrice());
        Destroy(gameObject);
        CloseUpgradeUI();
    }

    public int GetSellPrice()
    {
        return Mathf.RoundToInt(BaseSellPrice * Mathf.Pow(level, 0.8f));
    }

    // private void OnDrawGizmosSelected()
    // {
    //     Handles.color = Color.red;
    //     Handles.DrawWireDisc(transform.position, transform.forward, targetRange);
    // }

    private void ReplaceTower()
    {
        Destroy(towerObj);
        buildingManager.main.SetSelectedTower(2);
        towerObj = Instantiate(buildingManager.main.GetSelectedBuilding().prefab, transform.position, Quaternion.identity);
        Turret turret = towerObj.GetComponent<Turret>();
    }
}
