using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 25f;
    public float bulletLifetime = 5f;
    private float startTime;
    public Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = Time.time;

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = transform.right * bulletSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime > bulletLifetime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log(hitInfo.gameObject.name);

        /*
        if (hitInfo.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = hitInfo.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(1);
            }
        }
        */

        if(hitInfo.gameObject.name.Contains("Wall"))
        {
            Destroy(gameObject);
        }
        
    }


}
