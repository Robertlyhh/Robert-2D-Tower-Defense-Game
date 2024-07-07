using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : MonoBehaviour // EnergyBall is similar to arrow but slow down enemy when hit.
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float EnergyBallSpeed = 12f;
    [SerializeField] private float damgePoint = 10f;
    [SerializeField] private float slowAmount = 0.7f;
    [SerializeField] private float slowTime = 5f;

    private Transform target;
    private Vector2 lastDirection;
    private enemy_movement EnemyMovement;
    private int bulletType;

    private void Start()
    {
        bulletType = 2;
    }

    private void FixedUpdate()
    {
        if(gamePauseManager.isPaused)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        if(target == null)
        {   
            return;
        }
        Vector2 direction = (target.position - transform.position).normalized;
        lastDirection = direction;

        rb.velocity = direction * EnergyBallSpeed;
    }

    private void Update()
    {
        rotateTowardsTarget();
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
        transform.rotation = Quaternion.Euler(0, 0, angle + 180);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        other.gameObject.GetComponent<GoblinHp>().TakeDamage(damgePoint, bulletType);
        EnemyMovement = other.gameObject.GetComponent<enemy_movement>(); 
        StartCoroutine(EnemyMovement.slowed(slowTime, slowAmount));
        Debug.Log("EnergyBall hit");
        Destroy(gameObject);
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
