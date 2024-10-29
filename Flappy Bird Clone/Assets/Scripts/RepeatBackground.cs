using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 2.0f;  // Control the scroll speed in Inspector
    Vector3 startPos;
    float repeatWidth;
    public bool isScrolling;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        repeatWidth = GetComponent<BoxCollider2D>().size.x / 2;
        isScrolling = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isScrolling == true)
        {
            // Move the background left continuously
            transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);

            // If the background has moved beyond the repeat point
            if (transform.position.x < startPos.x - repeatWidth)
            {
                transform.position = startPos;
            }
        }
    }
}
