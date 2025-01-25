using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleMovement : MonoBehaviour
{
	[Header("Sound Detect Settings")]
	public AudioLoudnessDetection detector;
	public float loudnessSensibility = 100;
	
	[Header("Bubble Size Settings")]
	public float inflateRate = 0.6f; // Rate at which the bubble inflates
	public float shrinkRate = 0.1f; // Rate at which the bubble shrinks
	public float minSize = 0.3f; // Minimum size of the bubble
	public float maxSize = 1.5f; // Maximum size of the bubble
	public float warningThreshold = 0.7f; // Warning threshold as a percentage of max and min size

	[Header("Bubble Movement Settings")]
	public float speed = 1f;
	public float sizeFactor = 3f; // Factor affecting speed based on size
	public float minSpeed = 1f; // Minimum speed of the bubble
	public float maxSpeed = 5f; // Maximum speed of the bubble
	public float blowForce = 50f; // Force applied when blowing the bubble

	[Header("Bubble Wobble Settings")]
	public float wobbleFrequency = 1f; // Frequency of the wobble effect
	public float wobbleAmplitude = 0.3f; // Amplitude of the wobble effect

	[Header("Physics Settings")]
	public float inertiaDampening = 0.99f; // Factor to dampen the inertia over time

	[Header("Debug Info")]
	[SerializeField] private float currentSpeed; // Current speed visible in the inspector
	[SerializeField] private float currentSize; // Current size visible in the inspector

	private Transform bubbleTransform;
	private Vector3 wobbleOffset;
	private Vector3 velocity; // Stores the current velocity for inertia
	private float wobbleTime;
	private bool hasPopped = false; // Tracks if the bubble has popped

	void Start()
	{
		bubbleTransform = transform;
		velocity = Vector3.zero;
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			GameManager.Instance.PauseGame();
		}
		if(GameManager.Instance.IsGameRunning)
		{
			BubbleMove();
		}
	}
	
	void BubbleMove()
	{
		// Get the mouse position in world coordinates
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		// Ensure the Z-coordinate is zero for 2D
		mousePosition.z = 0;

		//TODO: 更改吹大气球逻辑
		// Adjust bubble size when holding left mouse button and space bar
		if (IsMouseOverCollider())
		{
			if (Input.GetKey(KeyCode.Space) && Input.GetMouseButton(0))
			{
				bubbleTransform.localScale += Vector3.one * inflateRate * Time.deltaTime;
			}
		}
		else
		{
			bubbleTransform.localScale -= Vector3.one * shrinkRate * Time.deltaTime;
		}

		// Update currentSize for debugging
		currentSize = bubbleTransform.localScale.x;

		// Calculate consistent warning offset
		float warningOffset = (maxSize - minSize) * (1f - warningThreshold);

		// Clamp the size to prevent exceeding maxSize or dropping below minSize
		if (bubbleTransform.localScale.x >= maxSize)
		{
			bubbleTransform.localScale = Vector3.one * maxSize;
			ChangeBubbleColor(Color.red); // Bubble pops when max size is reached
			if (!hasPopped)
			{
				Debug.Log("Bubble popped due to exceeding max size.");
				GameManager.Instance.EndGame();
				hasPopped = true;
			}
		}
		else if (bubbleTransform.localScale.x <= minSize)
		{
			bubbleTransform.localScale = Vector3.one * minSize;
			ChangeBubbleColor(Color.red); // Bubble pops when min size is reached
			if (!hasPopped)
			{
				Debug.Log("Bubble popped due to falling below min size.");
				GameManager.Instance.EndGame();
				hasPopped = true;
			}
		}
		else if (bubbleTransform.localScale.x >= maxSize - warningOffset || bubbleTransform.localScale.x <= minSize + warningOffset)
		{
			ChangeBubbleColor(Color.yellow); // Warning color
			hasPopped = false; // Reset pop state during warning
		}
		else
		{
			ChangeBubbleColor(Color.white); // Default color
			hasPopped = false; // Reset pop state during normal conditions
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

		//TODO:Embed 吹气逻辑
		// if (Input.GetMouseButton(0) && !IsMouseOverCollider())
		// {
		// 	// Calculate the direction away from the mouse cursor
		// 	Vector3 directionAwayFromMouse = (transform.position - mousePosition).normalized;

		// 	float blowForce = GetBlowForceByLoudness();
		// 	// Apply a force to simulate blowing the bubble
		// 	velocity += directionAwayFromMouse * blowForce * Time.deltaTime;
		// 	//Debug.Log("Blow Force: " + blowForce); //Log Blow Force		
		// }
		
		if (Input.GetKey(KeyCode.Space) && Input.GetMouseButton(0) && !IsMouseOverCollider())
		{
			// Calculate the direction away from the mouse cursor
			Vector3 directionAwayFromMouse = (transform.position - mousePosition).normalized;

			float blowForce = 50;
			// Apply a force to simulate blowing the bubble
			velocity += directionAwayFromMouse * blowForce * Time.deltaTime;
			//Debug.Log("Blow Force: " + blowForce); //Log Blow Force		
		}

		// Apply inertia and wobble to the bubble's movement
		transform.position += (velocity + wobbleOffset) * Time.deltaTime;

		// Dampening the inertia over time
		velocity *= inertiaDampening;

		if (!Input.GetMouseButton(0))
		{
			//Debug.Log("currentSpeed Velocity add: " + currentSpeed);
			// Move the bubble upward with inertia and wobble effect
			velocity += Vector3.up * currentSpeed * Time.deltaTime;
		}
	}
	
	private void ChangeBubbleColor(Color color)
	{
		SpriteRenderer bubbleRenderer = GetComponent<SpriteRenderer>();
		if (bubbleRenderer != null)
		{
			bubbleRenderer.color = color;
		}
	}
	
	float GetBlowForceByLoudness()
	{
		float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensibility;
		//Debug.Log("Loudness: " + loudness); //TODO:Log Loudness
		
		if(loudness < 0.1)
		{
			return 0;
		}
		else if(loudness < 1)
		{
			return 25f;
		}
		else if(loudness < 2)
		{
			return 50f;
		}
		else if(loudness < 3)
		{
			return 75f;
		}
		else if(loudness < 5)
		{
			return 100f;
		}
		else
		{
			return 125f;
		}
	}
	
	private bool IsMouseOverCollider()
	{
		Collider2D collider = GetComponent<Collider2D>();
		if (collider == null) return false;

		// Convert mouse screen position to world position
		Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		// Check if the mouse world position overlaps with the Collider2D
		return collider.OverlapPoint(mouseWorldPos);
	}
	
	public void SetHasPopped(bool p)
	{
		hasPopped = p;
	}
}
