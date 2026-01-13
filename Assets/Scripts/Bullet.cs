using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 25f;
    public float bulletLifetime = 5f;
    private float startTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = Time.time;

        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime * bulletSpeed);
        if (Time.time - startTime > bulletLifetime)
        {
            Destroy(gameObject);
        }

        

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }


}
