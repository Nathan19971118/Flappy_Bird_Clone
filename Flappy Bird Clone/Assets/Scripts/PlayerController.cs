using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 3f;
    public float maxYPosition = 1.28f;  // Adjust this to set the upper boundary
    public float minYPosition = -1.28f; // Adjust this to set the lower boundary

    float initialX;
    Rigidbody2D playerRb;
    Animator playerAnim;
    public bool isAlive = true;

    RepeatBackground background; // Changed to array to handle multiple backgrounds

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        playerAnim.SetBool("isFlap", true);
        initialX = transform.position.x; // Store the initial x-position

        // Find the RepeatBackground component in the scene
        background = FindObjectOfType<RepeatBackground>();

        // Add error checking
        if (background == null)
        {
            Debug.LogWarning("RepeatBackground component not found in the scene!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            Jump();
        }

        // Ensure the player stays at the initial x-position
        Vector3 pos = transform.position;
        pos.x = initialX;
        pos.y = Mathf.Clamp(pos.y, minYPosition, maxYPosition);
        transform.position = pos;
    }
    void Jump()
    {
        playerRb.velocity = new Vector2(0, jumpForce); // Only apply vertical force
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name +
                  " (Layer: " + LayerMask.LayerToName(collision.gameObject.layer) + ")");

        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Obstacle"))
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        isAlive = false;
        playerAnim.SetBool("isFlap", false);
        Debug.Log("Player is no longer alive!");

        // Add null check before calling StopScrolling
        if (background != null)
        {
            background.StopScrolling();
        }

        // Add additional game over logic here (UI, sounds, etc.)
    }
}
