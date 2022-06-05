using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagSpawner : MonoBehaviour
{
    public static BagSpawner Instance;

    public Bag _p1BagPrefab;
    public Bag _p2BagPrefab;

    public Transform p1Bag1SpawnPoint;
    public Transform p1Bag2SpawnPoint;
    public Transform p1Bag3SpawnPoint;
    public Transform p1Bag4SpawnPoint;

    public Transform p2Bag1SpawnPoint;
    public Transform p2Bag2SpawnPoint;
    public Transform p2Bag3SpawnPoint;
    public Transform p2Bag4SpawnPoint;


    void Awake()
    {
        Instance = this;
    }

    public void SpawnBags()
    {
        Bag p1b1 = Instantiate(_p1BagPrefab, p1Bag1SpawnPoint.position, p1Bag1SpawnPoint.rotation, null) as Bag;
        p1b1.initialSpawnPoint = p1Bag1SpawnPoint;

        Bag p1b2 = Instantiate(_p1BagPrefab, p1Bag2SpawnPoint.position, p1Bag2SpawnPoint.rotation, null) as Bag;
        p1b2.initialSpawnPoint = p1Bag2SpawnPoint;

        Bag p1b3 = Instantiate(_p1BagPrefab, p1Bag3SpawnPoint.position, p1Bag3SpawnPoint.rotation, null) as Bag;
        p1b3.initialSpawnPoint = p1Bag3SpawnPoint;

        Bag p1b4 = Instantiate(_p1BagPrefab, p1Bag4SpawnPoint.position, p1Bag4SpawnPoint.rotation, null) as Bag;
        p1b4.initialSpawnPoint = p1Bag4SpawnPoint;

        Bag p2b1 = Instantiate(_p2BagPrefab, p2Bag1SpawnPoint.position, p2Bag1SpawnPoint.rotation, null) as Bag;
        p2b1.initialSpawnPoint = p2Bag1SpawnPoint;

        Bag p2b2 = Instantiate(_p2BagPrefab, p2Bag2SpawnPoint.position, p2Bag2SpawnPoint.rotation, null) as Bag;
        p2b2.initialSpawnPoint = p2Bag2SpawnPoint;

        Bag p2b3 = Instantiate(_p2BagPrefab, p2Bag3SpawnPoint.position, p2Bag3SpawnPoint.rotation, null) as Bag;
        p2b3.initialSpawnPoint = p2Bag3SpawnPoint;

        Bag p2b4 = Instantiate(_p2BagPrefab, p2Bag4SpawnPoint.position, p2Bag4SpawnPoint.rotation, null) as Bag;
        p2b4.initialSpawnPoint = p2Bag4SpawnPoint;
    }
}
