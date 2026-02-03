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

    private Transform playerTransform;

    private Rigidbody2D rigidBody;
    private Vector2 targetDirection;

    private void Awake()
    {
        playerTransform = FindAnyObjectByType<PlayerScript>().transform;

        rigidBody = GetComponent<Rigidbody2D>();
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
        }
        else
        {
            //replace with wandering logic later
            targetDirection = Vector2.zero;
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
