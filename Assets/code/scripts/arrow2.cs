// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class arrow2 : MonoBehaviour
// {
//     [Header("References")]
//     [SerializeField] private Rigidbody2D rb;

//     [Header("Attributes")]
//     [SerializeField] private float arrowSpeed = 10f;
//     [SerializeField] private float damgePoint = 70f;

//     private Transform target;
//     private Vector2 lastDirection;

    
//     private void FixedUpdate()
//     {
//         if(gamePauseManager.isPaused)
//         {
//             rb.velocity = Vector2.zero;
//             return;
//         }
//         if(target == null)
//         {   
//             Destroy(gameObject);
//             return;
//         }
//         Vector2 direction = (target.position - transform.position).normalized;
//         lastDirection = direction;

//         rb.velocity = direction * arrowSpeed;
//     }

//     private void Update()
//     {
//         rotateTowardsTarget();
//         CheckArrowIsOffScreen();
//     }

//     public void SetTarget(Transform _target)
//     {
//         if(target != _target)
//         {
//             target = _target;
//         }
//     }

//     private void rotateTowardsTarget()
//     {
//         if(target == null)
//         {
//             return;
//         }

//         float angle = Mathf.Atan2(lastDirection.y, lastDirection.x) * Mathf.Rad2Deg;
//         transform.rotation = Quaternion.Euler(0, 0, angle + 180);
//     }

//     private void OnCollisionEnter2D(Collision2D other)
//     {
//         other.gameObject.GetComponent<GoblinHp>().TakeDamage(damgePoint);
//         Destroy(gameObject);
//     }

//     private void CheckArrowIsOffScreen()
//     {
//         if(transform.position.x > 40 || transform.position.x < -40 || transform.position.y > 40 || transform.position.y < -40)
//         {
//             Destroy(gameObject);
//         }
//     }
// }
