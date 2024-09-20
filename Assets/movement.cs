using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float horizontalSpeed = 3f;      // Speed of horizontal movement
    public float verticalAmplitude = 1f;    // Amplitude of vertical movement
    public float verticalFrequency = 1f;    // Frequency of vertical movement
    public float verticalOffset = 0f;       // Offset for unique vertical paths

    [Header("Appearance Settings")]
    public bool flipSprite = false;         // Option to flip sprite on X-axis

    [Header("Start Settings")]
    public StartSide startSide = StartSide.Left; // Choose starting side

    private SpriteRenderer spriteRenderer;
    private float screenHalfWidth;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool movingRight = true;

    // Enumeration for start side selection
    public enum StartSide
    {
        Left,
        Right
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Flip the sprite if required
        if (flipSprite)
        {
            spriteRenderer.flipX = true;
        }

        // Calculate half the screen width based on the camera's orthographic size
        Camera cam = Camera.main;
        screenHalfWidth = cam.orthographicSize * cam.aspect;

        // Set initial position and movement direction based on the selected start side
        if (startSide == StartSide.Left)
        {
            startPosition = new Vector3(-screenHalfWidth - GetComponent<SpriteRenderer>().bounds.size.x, transform.position.y, transform.position.z);
            movingRight = true;
        }
        else
        {
            startPosition = new Vector3(screenHalfWidth + GetComponent<SpriteRenderer>().bounds.size.x, transform.position.y, transform.position.z);
            movingRight = false;
            horizontalSpeed = Mathf.Abs(horizontalSpeed) * -1; // Ensure speed is negative for left movement
        }

        transform.position = startPosition;
    }

    void Update()
    {
        // Move horizontally
        transform.position += Vector3.right * horizontalSpeed * Time.deltaTime;

        // Apply vertical sinusoidal movement with offset
        float newY = Mathf.Sin(Time.time * verticalFrequency + verticalOffset) * verticalAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Check if the ghost has moved beyond the screen boundaries
        if (movingRight && transform.position.x > screenHalfWidth + GetComponent<SpriteRenderer>().bounds.size.x)
        {
            FlipDirection();
        }
        else if (!movingRight && transform.position.x < -screenHalfWidth - GetComponent<SpriteRenderer>().bounds.size.x)
        {
            FlipDirection();
        }
    }

    // Method to flip the movement direction and sprite
    void FlipDirection()
    {
        movingRight = !movingRight;
        horizontalSpeed = -horizontalSpeed;

        // Flip the sprite horizontally
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
}
