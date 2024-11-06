using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    [Header("Scroll Settings")]
    [SerializeField] float scrollSpeed = 0.5f;  // Control the scroll speed in Inspector
    Vector3 startPos;
    float repeatWidth;
    public bool isScrolling;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        var collider = GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            repeatWidth = collider.size.x / 2;
        }
        else
        {
            Debug.LogError("BoxCollider2D not found on RepeatBackground!");
        }
        isScrolling = false; // Start with scrolling disabled
    }

    // Update is called once per frame
    void Update()
    {
        // Only scroll if game is active and scrolling is enabled
        if (GameManager.Instance != null && 
            GameManager.Instance.isGameStarted && 
            !GameManager.Instance.isGamePaused && 
            isScrolling)
        {
            // Move the background left
            transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);

            // Reset position when beyond repeat point
            if (transform.position.x < startPos.x - repeatWidth)
            {
                transform.position = startPos;
            }
        }
    }
}
