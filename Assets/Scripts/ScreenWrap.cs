using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
	private float screenWidth; // Horizontal world bounds
	private float screenHeight; // Vertical world bounds

	void Start()
	{
		// Dynamically calculate screen bounds based on the camera
		Camera mainCamera = Camera.main;
		screenHeight = mainCamera.orthographicSize * 2f; // Vertical size is twice the orthographic size
		screenWidth = screenHeight * mainCamera.aspect; // Horizontal size depends on aspect ratio
	}

	void Update()
	{
		Vector3 position = transform.position;

		// Check horizontal bounds and wrap smoothly
		if (position.x > screenWidth / 2f)
		{
			position.x = -screenWidth / 2f + 0.1f; // Add slight offset for smooth transition
		}
		else if (position.x < -screenWidth / 2f)
		{
			position.x = screenWidth / 2f - 0.1f; // Add slight offset for smooth transition
		}

		// Prevent the bubble from moving below the bottom of the screen
		float cameraBottomY = Camera.main.transform.position.y - screenHeight / 2f;
		if (position.y < cameraBottomY)
		{
			position.y = cameraBottomY; // Clamp to the bottom of the screen
			Debug.Log("Bubble popped due to touch the bottom.");
			GameManager.Instance.EndGame();
			GetComponent<BubbleMovement>().SetHasPopped(true);
		}

		// Update the transform position
		transform.position = position;
	}
}
