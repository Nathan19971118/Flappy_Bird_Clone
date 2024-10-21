using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public float spawnX = 10f;
    public float minY = -1f;
    public float maxY = 3f;
    float startDelay = 2f;
    float repeatRate = 2f;

    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        InvokeRepeating("SpawnObstacles", startDelay, repeatRate);
    }

    void SpawnObstacles()
    {
        if (playerController.isAlive)
        {
            float randomY = Random.Range(minY, maxY);
            Vector3 spawnPos = new Vector3(spawnX, randomY, 0);
            Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);
        }
    }
}
