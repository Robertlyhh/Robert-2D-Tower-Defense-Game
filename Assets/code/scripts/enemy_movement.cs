using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_movement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float speed = 4f;

    private Transform target;
    private int wavepointIndex = 0;
    private float baseSpeed;

    private void Start()
    {
        baseSpeed = speed;
        target = levelManager.main.path[wavepointIndex];

    }

    private void Update()
    {

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            if (wavepointIndex == levelManager.main.path.Length)
            {
                enemySpawner.onEnemyDeath.Invoke();
                Destroy(gameObject);
                levelManager.main.RemoveLife(1);
                return;
            }else
            {
                target = levelManager.main.path[wavepointIndex];
                wavepointIndex++;
            }
        }


    }

    private void FixedUpdate()
    {
        if (gamePauseManager.isPaused)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        Vector2 dir = target.position - transform.position;
        rb.velocity = dir.normalized * speed;
    }

    public void Slow(float slowAmount)
    {
        speed = baseSpeed * (1f - slowAmount);
    }

    public void ResetSpeed()
    {
        speed = baseSpeed;
    }

    public IEnumerator slowed(float slowTime, float slowAmount)
    {
        Slow(slowAmount);
        yield return new WaitForSeconds(slowTime);
        ResetSpeed();
    }

}
