using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // The object the camera should follow (e.g., the bubble)
    public float smoothSpeed = 5f; // Smoothness of the camera movement
    public float yOffset = -2f; // Vertical offset to keep the target slightly below center

    private float lowestY; // Tracks the lowest Y position the camera has reached

    void Start()
    {
        // Initialize the lowest Y position
        if (target != null)
        {
            lowestY = target.position.y + yOffset;
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Calculate the target camera position
            Vector3 targetPosition = new Vector3(transform.position.x, target.position.y + yOffset, transform.position.z);

            // Ensure the camera does not move downward past the lowest Y position
            if (targetPosition.y < lowestY)
            {
                targetPosition.y = lowestY;
            }
            else
            {
                // Update the lowest Y position if the camera moves up
                lowestY = targetPosition.y;
            }

            // Smoothly move the camera to the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }
}
