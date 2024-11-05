using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float leftBound = -1.5f;

    // Update is called once per frame
    void Update()
    {
        // Only move if game is active
        if (GameManager.Instance != null && GameManager.Instance.isGameStarted && !GameManager.Instance.isGamePaused)
        {
            transform.Translate(Vector2.left * Time.deltaTime * speed);

            // Destroy obstacles that are out of bounds
            if (transform.position.x < leftBound && 
                (gameObject.CompareTag("Pipe") && gameObject.CompareTag("Score Zone")))
            {
                Destroy(gameObject);
            }
        }
    }
}
