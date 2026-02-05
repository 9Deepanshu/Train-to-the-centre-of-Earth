using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private float secondsToNextSpawn = 1.0f;
    private float nextSpawnTime = 0.0f;
    [SerializeField] private List<GameObject> Enemies;
    [SerializeField] private List<GameObject> DoorSpawnPoints;
    [SerializeField] public List<GameObject> WanderPoints;

    private void Awake()
    {
        WanderPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("WanderPoint"));
        DoorSpawnPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("SpawnPointDoor"));
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            //Instantiate(Enemies[Random.Range(0, Enemies.Count)], DoorSpawnPoints[Random.Range(0, DoorSpawnPoints.Count)].transform.position, Quaternion.identity);
            GameObject enemy = Instantiate(Enemies[0], DoorSpawnPoints[Random.Range(0, DoorSpawnPoints.Count)].transform.position, Quaternion.identity);
            //make sure enemy is facing towards center
            if (Vector2.Angle(enemy.transform.up, (Vector2.zero - (Vector2)enemy.transform.position)) > 90)
            {
                enemy.transform.Rotate(0, 0, 180);
            }
            nextSpawnTime = Time.time + secondsToNextSpawn;
        }
    }
}
