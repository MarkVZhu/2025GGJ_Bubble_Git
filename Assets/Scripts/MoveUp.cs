using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUp : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        // Get the mouse position in world coordinates (2D)
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Ensure the Z-axis is set to 0 for 2D games
        mousePosition.z = 0;

        // Debug: Print the mouse position to confirm it updates correctly
        Debug.Log("Mouse Position: " + mousePosition);

        // Move the bubble towards the mouse cursor position
        transform.position = Vector2.MoveTowards(transform.position, mousePosition, speed * Time.deltaTime);
    }
}
