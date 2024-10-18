using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 5f;
    public float gravity = -9.8f;

    Vector2 velocity;
    public bool isAlive = true;

    [SerializeField]
    private bool useMouseInput = true;
    [SerializeField]
    private bool useKeyboardInput = true;
    [SerializeField]
    private bool useTouchInput = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) return;

        //Check for touch input
        if (ShouldJump())
        {
            Jump();
        }

        //Apply gravity
        velocity.y += gravity * Time.deltaTime;

        //Move the bird
        transform.Translate(velocity * Time.deltaTime);
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
        velocity.y = jumpForce;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isAlive = false;

        //Game Over logic
    }
}
