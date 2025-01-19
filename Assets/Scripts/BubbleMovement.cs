using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleMovement : MonoBehaviour
{
    public float speed = 2f;
    public float inflateRate = 0.6f; // Rate at which the bubble inflates
    public float shrinkRate = 0.1f; // Rate at which the bubble shrinks
    public float minSize = 0.1f; // Minimum size of the bubble
    public float sizeFactor = 3f; // Factor affecting speed based on size
    public float minSpeed = 1f; // Minimum speed of the bubble
    public float maxSpeed = 5f; // Maximum speed of the bubble
    public float blowForce = 50f; // Force applied when blowing the bubble
    public float wobbleFrequency = 1f; // Frequency of the wobble effect
    public float wobbleAmplitude = 0.3f; // Amplitude of the wobble effect
    public float inertiaDampening = 0.99f; // Factor to dampen the inertia over time

    [SerializeField] private float currentSpeed; // Current speed visible in the inspector

    private Transform bubbleTransform;
    private Vector3 wobbleOffset;
    private Vector3 velocity; // Stores the current velocity for inertia
    private float wobbleTime;

    void Start()
    {
        bubbleTransform = transform;
        velocity = Vector3.zero;
    }

    void Update()
    {
        // Get the mouse position in world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Ensure the Z-coordinate is zero for 2D
        mousePosition.z = 0;

        // Adjust bubble size when holding left mouse button and space bar
        if (Input.GetMouseButton(0))
        {
            if (Input.GetKey(KeyCode.Space))
            {
                bubbleTransform.localScale += Vector3.one * inflateRate * Time.deltaTime;
            }
        }
        else
        {
            bubbleTransform.localScale -= Vector3.one * shrinkRate * Time.deltaTime;

            // Clamp the size to the minimum size
            if (bubbleTransform.localScale.x < minSize)
                bubbleTransform.localScale = Vector3.one * minSize;
        }

        // Adjust speed based on size (smaller bubbles float faster)
        currentSpeed = speed + (1f / bubbleTransform.localScale.x) * sizeFactor;

        // Clamp speed to min and max values
        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);

        // Add a wobble effect for a more natural bubble movement
        wobbleTime += Time.deltaTime;
        wobbleOffset = new Vector3(
            Mathf.Sin(wobbleTime * wobbleFrequency) * wobbleAmplitude,
            Mathf.Cos(wobbleTime * wobbleFrequency) * wobbleAmplitude,
            0
        );

        if (Input.GetKey(KeyCode.Space) && !Input.GetMouseButton(0))
        {
            // Calculate the direction away from the mouse cursor
            Vector3 directionAwayFromMouse = (transform.position - mousePosition).normalized;

            // Apply a force to simulate blowing the bubble
            velocity += directionAwayFromMouse * blowForce * Time.deltaTime;
        }

        // Apply inertia and wobble to the bubble's movement
        transform.position += (velocity + wobbleOffset) * Time.deltaTime;

        // Dampening the inertia over time
        velocity *= inertiaDampening;

        if (!Input.GetKey(KeyCode.Space))
        {
            // Move the bubble upward with inertia and wobble effect
            velocity += Vector3.up * currentSpeed * Time.deltaTime;
        }
    }
}
