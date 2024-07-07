using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject[] bulletPrefabs;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private GameObject upgradeButton;
    [SerializeField] private GameObject sellButton;


    [Header("Attributes")]
    [SerializeField] private float targetRange = 3f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private int BaseUpgradeCost = 100;
    [SerializeField] private int BaseSellPrice = 50;

    [Header("Visuals")]
    [SerializeField] private Sprite[] upgradeSprites;

    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip[] audioClips;
    public AudioClip arrowClip;



    private SpriteRenderer sr;
    private Transform target;
    private float fireTimer;
    private int level = 1;
    private float targetRangeBase;
    private GameObject towerObj;
    private Turret turret;
    public LineRenderer lineRenderer;
    private bool targetRangeVisible = false;
    private GameObject bullet;
    private arrow arrowScript;
    private System.Random random = new System.Random();

    private void Start()
    {
        targetRangeBase = targetRange;
        upgradeButton.GetComponent<Button>().onClick.AddListener(Upgrade);
        sellButton.GetComponent<Button>().onClick.AddListener(Sell);
        sr = GetComponentInChildren<SpriteRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
        bullet = bulletPrefabs[level - 1];

        audioSource = GetComponent<AudioSource>();
        int i = random.Next(0, audioClips.Length);
        audioSource.PlayOneShot(audioClips[i]);
    }

    private void Update()
    {
        if (levelManager.main.gameIsOver || gamePauseManager.isPaused)
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
        if (bullet == null)
        {
            Debug.LogError("bulletPrefab is not assigned.");
            return;
        }

        if (firingPoint == null)
        {
            Debug.LogError("firingPoint is not assigned.");
            return;
        }

        GameObject arrowObj = Instantiate(bullet, firingPoint.position, Quaternion.identity);
        audioSource.PlayOneShot(arrowClip);

        if (arrowObj != null)
        {

            arrowScript = arrowObj.GetComponent<arrow>();
            
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
        if(level >= 2)
        {
            Debug.Log("Max level reached");
            return;
        }
        if(levelManager.main.currency < GetUpgradeCost())
        {
            Debug.Log("Not enough currency");
            return;
        }

        levelManager.main.RemoveCurrency(GetUpgradeCost());
        targetRange = GetTargetRange();
        sr.sprite = upgradeSprites[level - 1];
        level++;
        bullet.GetComponent<arrow>().SetDamage(GetArrowDamage());
        int i = random.Next(0, audioClips.Length);
        audioSource.PlayOneShot(audioClips[i]);



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

    private float GetArrowDamage()
    {
        return Mathf.RoundToInt(50 * Mathf.Pow(level, 0.8f));
    }


    public void SetupLineRenderer()
    {
        int segments = 360;
        lineRenderer.positionCount = segments + 1;
        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        float angle = 0f;
        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * targetRange;
            float z = Mathf.Cos(Mathf.Deg2Rad * angle) * targetRange;
            lineRenderer.SetPosition(i, new Vector3(x, z, 0));
            angle += (360f / segments);
        }
        Debug.Log("Line renderer setup");
    }

}
