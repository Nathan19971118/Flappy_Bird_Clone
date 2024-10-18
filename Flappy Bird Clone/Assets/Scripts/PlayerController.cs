using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 5f;
    float matHeight = 0.72f;

    Rigidbody2D playerRb;
    Vector2 velocity;
    public bool isAlive = true;
    public bool hasStarted = false;

    [SerializeField]
    private bool useMouseInput = true;
    [SerializeField]
    private bool useKeyboardInput = true;
    [SerializeField]
    private bool useTouchInput = true;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerRb.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) return;

        if (!hasStarted)
        {
            //Wait for the first jump input to start the game
            if (ShouldJump())
            {
                StartGame();
            }
            return;
        }

        //Check for jump input evert frame
        if (ShouldJump())
        {
            Jump();
        }
    }

    void StartGame()
    {
        hasStarted = true;
        playerRb.gravityScale = 1;
        Jump();
    }

    bool ShouldJump()
    {
        //Check mouse input (Left click)
        if (useMouseInput && Input.GetMouseButtonDown(0))
        {
            return true;
        }

        //Check keyboard input (Space Bar)
        if (useKeyboardInput && Input.GetKeyDown(KeyCode.Space))
        {
            return true;
        }

        //Check touch input
        if (useTouchInput && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                return true;
            }
        }

        return false;
    }

    void Jump()
    {
        playerRb.velocity = Vector2.up * jumpForce;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isAlive = false;

        //Game Over logic
    }
}
