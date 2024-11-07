using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject obstaclePrefab;
    public float spawnX = 10f;
    public float minY = -1f;
    public float maxY = 3f;
    float startDelay = 1f;
    float repeatRate = 0.7f;

    PlayerController playerController;
    bool isSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player")?.GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("Player not found! Make sure your player object is named 'Player'");
        }
    }

    public void SpawnObstacles()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            InvokeRepeating(nameof(SpawnObstacle), startDelay, repeatRate);
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
        CancelInvoke(nameof(SpawnObstacle));
    }

    private void SpawnObstacle()
    {
        if (playerController != null && playerController.isAlive && GameManager.Instance.isGameStarted)
        {
            float randomY = Random.Range(minY, maxY);
            Vector3 spawnPos = new Vector3(spawnX, randomY, 0);
            Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);
        }
        else
        {
            StopSpawning();
        }
    }

    public void DestroyObjects()
    {
        if (playerController.isAlive == true)
        {
            // Find all objects tagged as "Pipe"
            GameObject[] pipes = GameObject.FindGameObjectsWithTag("Pipe");

            // Find all objects tagged as "Score Zone"
            GameObject[] scoreZones = GameObject.FindGameObjectsWithTag("Score Zone");

            // Loop through each pipe and destroy it
            foreach (GameObject pipe in pipes)
            {
                Destroy(pipe);
            }

            // Loop through each pipe and destroy it
            foreach (GameObject scoreZone in scoreZones)
            {
                Destroy(scoreZone);
            }
        }
    }
}
