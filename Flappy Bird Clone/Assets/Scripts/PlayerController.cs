using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float jumpForce = 3f;
    public float maxYPosition = 1.28f;  // Adjust this to set the upper boundary
    public float minYPosition = -1.28f; // Adjust this to set the lower boundary

    [Header("Components")]
    Rigidbody2D playerRb;
    Animator playerAnim;
    float initialX;
    public bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        initialX = transform.position.x; // Store the initial x-position

        // Initialize animation but don't start flapping until game starts
        playerAnim.SetBool("isFlap", false);

        // Freeze the rigidbody until game starts
        playerRb.simulated = false;

        // Start with player inactive
        isAlive = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Only process input if game is started and player is alive
        if (GameManager.Instance.isGameStarted && !GameManager.Instance.isGamePaused && isAlive)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                Jump();
            }
        }

        // Keep player within bounds
        Vector3 pos = transform.position;
        pos.x = initialX;
        pos.y = Mathf.Clamp(pos.y, minYPosition, maxYPosition);
        transform.position = pos;
    }

    public void StartPlaying()
    {
        playerRb.simulated = true;
        playerAnim.SetBool("isFlap", true);
        isAlive = true;
    }

    void Jump()
    {
        playerRb.velocity = new Vector2(0, jumpForce); // Only apply vertical force
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Pipe"))
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Add score when passing through pipe scoring trigger
        if (other.CompareTag("Score Zone"))
        {
            GameManager.Instance.OnPipePass();
        }
    }

    private void Die()
    {
        if (isAlive)
        {
            isAlive = false;
            playerAnim.SetBool("isFlap", false);
            GameManager.Instance.GameOver();
        }
    }
}
