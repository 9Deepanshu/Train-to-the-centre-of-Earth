using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
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
        
    }
}
