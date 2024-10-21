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
    public bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        initialX = transform.position.x; // Store the initial x-position
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
            isAlive = false;
            Debug.Log("Player is no longer alive!");
            // Implement game over logic here
        }
    }
}
