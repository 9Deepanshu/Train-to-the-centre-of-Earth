using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int enemyHealth = 50;
    public int enemyDamage = 10;
    public float damageInterval = 1.0f;
    private float nextDamageTime = 0.0f;

    private bool playerInRange = false;
    private Vector2 directionToPlayer;

    [Header("Movement Settings")]
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float rotationSpeed = 3.0f;
    [SerializeField] private float playerAwarenessRadius = 45.0f;
    [SerializeField] private float playerAwarenessDistance = 10.0f;
    [SerializeField] private float secondsToWalkThroughDoor = 3.0f;
    private float startTime;

    private Transform playerTransform;

    private Rigidbody2D rigidBody;
    private Vector2 targetDirection;
    private Vector2 targetPoint;

    private List<GameObject> wanderPoints;

    private void Awake()
    {
        playerTransform = FindAnyObjectByType<PlayerScript>().transform;

        rigidBody = GetComponent<Rigidbody2D>();

        
    }

    private void Start()
    {
        Collider2D collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
        startTime = Time.time;

        wanderPoints = FindAnyObjectByType<SpawnManager>().WanderPoints;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 enemyToPlayerVector = playerTransform.position - transform.position;
        directionToPlayer = enemyToPlayerVector.normalized;

        if (enemyToPlayerVector.magnitude <= playerAwarenessDistance && Vector2.Angle(transform.up,enemyToPlayerVector) <= playerAwarenessRadius)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }

        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
        }

        if (Time.time >= startTime + secondsToWalkThroughDoor)
        {
            Collider2D collider = GetComponent<Collider2D>();
            collider.isTrigger = false;
        }
    }

    private void FixedUpdate()  
    {
        UpdateTargetDirection();
        RotateTowardsTarget();
        SetVelocity();
    }

    private void UpdateTargetDirection()
    {
        if(playerInRange)
        {
            targetDirection = directionToPlayer;
            targetPoint = playerTransform.position;
        }
        else
        {
            if (Vector2.Distance(transform.position, targetPoint) < 1.0f)
            {
                targetDirection = DirectionToNextWanderPoint();
                
            }
            else            
            {
                targetDirection = (targetPoint - (Vector2)transform.position).normalized;
            }

        }
    }

    private Vector2 DirectionToNextWanderPoint()
    {
        Vector2 nearestWanderPoint;
        Vector2 secondNearestWanderPoint;
        if (Vector2.Distance(transform.position, wanderPoints[0].transform.position) < Vector2.Distance(transform.position, wanderPoints[1].transform.position))
        {
            nearestWanderPoint = wanderPoints[0].transform.position;
            secondNearestWanderPoint = wanderPoints[1].transform.position;
        }
        else
        {
            nearestWanderPoint = wanderPoints[1].transform.position;
            secondNearestWanderPoint = wanderPoints[0].transform.position;
        }

        for (int i = 2; i < wanderPoints.Count; i++)
        {

            if (Vector2.Distance(transform.position, wanderPoints[i].transform.position) < Vector2.Distance(transform.position, nearestWanderPoint))
            {
                nearestWanderPoint = wanderPoints[i].transform.position;
            }
            else if (Vector2.Distance(transform.position, wanderPoints[i].transform.position) < Vector2.Distance(transform.position, secondNearestWanderPoint))
            {
                secondNearestWanderPoint = wanderPoints[i].transform.position;
            }
        }

        if (Vector2.Angle(transform.up, (nearestWanderPoint - (Vector2)transform.position)) <= playerAwarenessRadius)
        {
            targetPoint = nearestWanderPoint;
            return (nearestWanderPoint - (Vector2)transform.position).normalized;
        }
        else if (Vector2.Angle(transform.up, (secondNearestWanderPoint - (Vector2)transform.position)) <= playerAwarenessRadius)
        {
            targetPoint = secondNearestWanderPoint;
            return (secondNearestWanderPoint - (Vector2)transform.position).normalized;
        }
        else
        {
            return (nearestWanderPoint - (Vector2)transform.position).normalized;
        }
    }

    private void RotateTowardsTarget()
    {
        //check this later
        if (targetDirection == Vector2.zero)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, targetDirection);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

        rigidBody.SetRotation(rotation);
    }

    private void SetVelocity()
    {
        //check this later
        if (targetDirection == Vector2.zero)
        {
            rigidBody.linearVelocity = Vector2.zero;
        }
        else
        {
            rigidBody.linearVelocity = transform.up * speed;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.name.Contains("Player") && Time.time >= nextDamageTime)
        {
            collision.gameObject.GetComponent<PlayerScript>().playerHealth -= enemyDamage;
            nextDamageTime = Time.time + damageInterval;
        }
    }
}
