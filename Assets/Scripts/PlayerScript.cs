using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    public GameObject gun;
    public GameObject bullet;
    private Rigidbody2D m_rigidbody;
    public float moveSpeed = 20f;
    public float gunOffset = 1f;
    private float gunCooldown = 0.2f;
    private float lastShotTime = 0f;

    private Vector3 bulletSpawnPoint;
    public float bulletOffsetFromGun = 1f;

    // These variables are to hold the Action references
    InputAction moveAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find the references to "Move" 
        moveAction = InputSystem.actions.FindAction("Move");

        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Camera follows
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);

        // Read the "Move" action value, which is a 2D vector

        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        m_rigidbody.MovePosition(m_rigidbody.position + moveValue * Time.deltaTime * moveSpeed);

        var mouse = Mouse.current;
        Vector2 gunPointDirection = Camera.main.ScreenToWorldPoint(mouse.position.ReadValue()) - transform.position;

        // Calculate angle
        float angle = Mathf.Atan2(gunPointDirection.y, gunPointDirection.x) * Mathf.Rad2Deg;

        // Rotate gun
        gun.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Move gun slightly forward in aiming direction
        Vector2 offsetDirection = gunPointDirection.normalized;

        gun.transform.position = (Vector2)transform.position + offsetDirection * gunOffset;
        bulletSpawnPoint = (Vector2)transform.position + offsetDirection * (gunOffset + bulletOffsetFromGun);

        if (mouse.leftButton.isPressed & Time.time > lastShotTime + gunCooldown)
        {
            Instantiate(bullet, bulletSpawnPoint, gun.transform.rotation);
            lastShotTime = Time.time;
        }
    }
}
