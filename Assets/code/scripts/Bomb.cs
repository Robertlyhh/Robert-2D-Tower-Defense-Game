using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask enemyMask;

    [Header("Attributes")]
    [SerializeField] private float arrowSpeed = 5f;
    [SerializeField] private float damgePoint = 50f;
    [SerializeField] private float peakHeight = 0.5f;

    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip[] audioClips;

    private Transform target;
    private Vector2 lastDirection;
    private int bulletType;
    private bool hasExploded = false;
    public GameObject explosion;
    public float targetRange = 1f;
    private float targetX;
    private float targetY;

    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        bulletType = 3;
        targetX = target.position.x;
        targetY = target.position.y;
        CalculateTrajectory();
    }

    private void CalculateTrajectory() 
    {
        float targetRange = Vector2.Distance(transform.position, target.position);
        if(transform.position.x > target.position.x)
        {
            targetRange = -targetRange;
        }
        float gravity = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);
        float verticalSpeed = Mathf.Sqrt(2 * gravity * peakHeight);
        float totalTime = 2 * verticalSpeed / gravity;
        float horizontalSpeed = targetRange / totalTime;

        Vector2 initialVelocity = new Vector2(horizontalSpeed, verticalSpeed);
        rb.velocity = initialVelocity;
    }

    private void FixedUpdate()
    {
        if(gamePauseManager.isPaused)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (rb.velocity.y < 0) 
        { // 当炸弹开始下落时
        rb.gravityScale = 2; // 增加重力影响，使抛物线更加明显（可选）
        }

        if(transform.position.y < targetY && !hasExploded && rb.velocity.y < 0)
        {
            Debug.Log("Explode");
            Explode();
        }
    }

    private void Update()
    {
        //rotateTowardsTarget();
        CheckArrowIsOffScreen();
    }

    public void SetTarget(Transform _target)
    {
        if(target != _target)
        {
            target = _target;
        }
    }

    private void rotateTowardsTarget()
    {
        if(target == null)
        {
            return;
        }

        float angle = Mathf.Atan2(lastDirection.y, lastDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     //audioSource.PlayOneShot(audioClips[0]);
    //     other.gameObject.GetComponent<GoblinHp>().TakeDamage(damgePoint, bulletType);
    //     Destroy(gameObject);
    // }

    private void Explode()
    {
        rb.velocity = Vector2.zero;
        hasExploded = true;
        Instantiate(explosion, transform.position, Quaternion.identity);
        explodeDamage();
        Destroy(gameObject);
    }

    private void explodeDamage()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetRange, (Vector2)
        transform.position, 0, enemyMask);

        if(hits.Length > 0)
        {
            foreach(RaycastHit2D hit in hits)
            {
                hit.transform.GetComponent<GoblinHp>().TakeDamage(damgePoint, bulletType);
            }
        }
    }

    private void CheckArrowIsOffScreen()
    {
        if(transform.position.x > 40 || transform.position.x < -40 || transform.position.y > 40 || transform.position.y < -40)
        {
            Destroy(gameObject);
        }
    }

    public void SetDamage(float damage)
    {
        damgePoint = damage;
    }
}
