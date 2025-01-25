// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class BubbleMovement : MonoBehaviour
// {
// 	[Header("Sound Detect Settings")]
// 	public AudioLoudnessDetection detector;
// 	public float loudnessSensibility = 100;
//
// 	[Header("Bubble Size Settings")]
// 	public float inflateRate = 0.6f; // Rate at which the bubble inflates
// 	public float shrinkRate = 0.1f; // Rate at which the bubble shrinks
// 	public float minSize = 0.3f; // Minimum size of the bubble
// 	public float maxSize = 1.5f; // Maximum size of the bubble
// 	public float warningThreshold = 0.7f; // Warning threshold as a percentage of max and min size
//
// 	[Header("Bubble Movement Settings")]
// 	public float speed = 1f;
// 	public float sizeFactor = 3f; // Factor affecting speed based on size
// 	public float minSpeed = 1f; // Minimum speed of the bubble
// 	public float maxSpeed = 5f; // Maximum speed of the bubble
// 	public float blowForce = 50f; // Force applied when blowing the bubble
//
// 	[Header("Blow Force Curve")]
// 	public AnimationCurve loudnessToForceCurve = AnimationCurve.EaseInOut(0f, 0f, 5f, 125f);
//
// 	[Header("Bubble Wobble Settings")]
// 	public float wobbleFrequency = 1f; // Frequency of the wobble effect
// 	public float wobbleAmplitude = 0.3f; // Amplitude of the wobble effect
//
// 	[Header("Physics Settings")]
// 	public float inertiaDampening = 0.99f; // Factor to dampen the inertia over time
//
// 	[Header("Debug Info")]
// 	[SerializeField] private float currentSpeed; // Current speed visible in the inspector
// 	[SerializeField] private float currentSize; // Current size visible in the inspector
//
// 	private Transform bubbleTransform;
// 	private Vector3 wobbleOffset;
// 	private Vector3 velocity; // Stores the current velocity for inertia
// 	private float wobbleTime;
// 	private bool hasPopped = false; // Tracks if the bubble has popped
//
// 	void Start()
// 	{
// 		bubbleTransform = transform;
// 		velocity = Vector3.zero;
// 	}
//
// 	void Update()
// 	{
// 		if(Input.GetKeyDown(KeyCode.Escape))
// 		{
// 			GameManager.Instance.PauseGame();
// 		}
// 		if(GameManager.Instance.IsGameRunning)
// 		{
// 			BubbleMove();
// 		}
// 	}
//
// 	void BubbleMove()
// 	{
// 		// Get the mouse position in world coordinates
// 		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//
// 		// Ensure the Z-coordinate is zero for 2D
// 		mousePosition.z = 0;
//
// 		//TODO: 更改吹大气球逻辑
// 		// Adjust bubble size when holding left mouse button and space bar
// 		if (IsMouseOverCollider())
// 		{
// 			if (Input.GetKey(KeyCode.Space) && Input.GetMouseButton(0))
// 			{
// 				bubbleTransform.localScale += Vector3.one * inflateRate * Time.deltaTime;
// 			}
// 		}
// 		else
// 		{
// 			bubbleTransform.localScale -= Vector3.one * shrinkRate * Time.deltaTime;
// 		}
//
// 		// Update currentSize for debugging
// 		currentSize = bubbleTransform.localScale.x;
//
// 		// Calculate consistent warning offset
// 		float warningOffset = (maxSize - minSize) * (1f - warningThreshold);
//
// 		// Clamp the size to prevent exceeding maxSize or dropping below minSize
// 		if (bubbleTransform.localScale.x >= maxSize)
// 		{
// 			bubbleTransform.localScale = Vector3.one * maxSize;
// 			ChangeBubbleColor(Color.red); // Bubble pops when max size is reached
// 			if (!hasPopped)
// 			{
// 				Debug.Log("Bubble popped due to exceeding max size.");
// 				GameManager.Instance.EndGame();
// 				hasPopped = true;
// 			}
// 		}
// 		else if (bubbleTransform.localScale.x <= minSize)
// 		{
// 			bubbleTransform.localScale = Vector3.one * minSize;
// 			ChangeBubbleColor(Color.red); // Bubble pops when min size is reached
// 			if (!hasPopped)
// 			{
// 				Debug.Log("Bubble popped due to falling below min size.");
// 				GameManager.Instance.EndGame();
// 				hasPopped = true;
// 			}
// 		}
// 		else if (bubbleTransform.localScale.x >= maxSize - warningOffset || bubbleTransform.localScale.x <= minSize + warningOffset)
// 		{
// 			ChangeBubbleColor(Color.yellow); // Warning color
// 			hasPopped = false; // Reset pop state during warning
// 		}
// 		else
// 		{
// 			ChangeBubbleColor(Color.white); // Default color
// 			hasPopped = false; // Reset pop state during normal conditions
// 		}
//
// 		// Adjust speed based on size (smaller bubbles float faster)
// 		currentSpeed = speed + (1f / bubbleTransform.localScale.x) * sizeFactor;
//
// 		// Clamp speed to min and max values
// 		currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);
//
// 		// Add a wobble effect for a more natural bubble movement
// 		wobbleTime += Time.deltaTime;
// 		wobbleOffset = new Vector3(
// 			Mathf.Sin(wobbleTime * wobbleFrequency) * wobbleAmplitude,
// 			Mathf.Cos(wobbleTime * wobbleFrequency) * wobbleAmplitude,
// 			0
// 		);
//
// 		//TODO:Embed 吹气逻辑
// 		// if (Input.GetMouseButton(0) && !IsMouseOverCollider())
// 		// {
// 		// 	// Calculate the direction away from the mouse cursor
// 		// 	Vector3 directionAwayFromMouse = (transform.position - mousePosition).normalized;
//
// 		// 	float blowForce = GetBlowForceByLoudness();
// 		// 	// Apply a force to simulate blowing the bubble
// 		// 	velocity += directionAwayFromMouse * blowForce * Time.deltaTime;
// 		// 	//Debug.Log("Blow Force: " + blowForce); //Log Blow Force
// 		// }
//
// 		if (Input.GetKey(KeyCode.Space) && Input.GetMouseButton(0) && !IsMouseOverCollider())
// 		{
// 			// Calculate the direction away from the mouse cursor
// 			Vector3 directionAwayFromMouse = (transform.position - mousePosition).normalized;
//
// 			float blowForce = 50;
// 			// float blowForce = GetBlowForceByLoudness();
// 			// Apply a force to simulate blowing the bubble
// 			velocity += directionAwayFromMouse * blowForce * Time.deltaTime;
// 			//Debug.Log("Blow Force: " + blowForce); //Log Blow Force
// 		}
//
// 		// Apply inertia and wobble to the bubble's movement
// 		transform.position += (velocity + wobbleOffset) * Time.deltaTime;
//
// 		// Dampening the inertia over time
// 		velocity *= inertiaDampening;
//
// 		if (!Input.GetMouseButton(0))
// 		{
// 			//Debug.Log("currentSpeed Velocity add: " + currentSpeed);
// 			// Move the bubble upward with inertia and wobble effect
// 			velocity += Vector3.up * currentSpeed * Time.deltaTime;
// 		}
// 	}
//
// 	private void ChangeBubbleColor(Color color)
// 	{
// 		SpriteRenderer bubbleRenderer = GetComponent<SpriteRenderer>();
// 		if (bubbleRenderer != null)
// 		{
// 			bubbleRenderer.color = color;
// 		}
// 	}
//
// 	// float GetBlowForceByLoudness()
// 	// {
// 	// 	float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensibility;
// 	// 	Debug.Log("Loudness: " + loudness); //TODO:Log Loudness
// 	//
// 	// 	if(loudness < 0.1)
// 	// 	{
// 	// 		return 0;
// 	// 	}
// 	// 	else if(loudness < 1)
// 	// 	{
// 	// 		return 25f;
// 	// 	}
// 	// 	else if(loudness < 2)
// 	// 	{
// 	// 		return 50f;
// 	// 	}
// 	// 	else if(loudness < 3)
// 	// 	{
// 	// 		return 75f;
// 	// 	}
// 	// 	else if(loudness < 5)
// 	// 	{
// 	// 		return 100f;
// 	// 	}
// 	// 	else
// 	// 	{
// 	// 		return 125f;
// 	// 	}
// 	// }
//
//
// 	float GetBlowForceByLoudness()
// 	{
// 		float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensibility;
//
// 		loudness = Mathf.Clamp(loudness, 0f, 10f);
//
// 		float blowForce = loudnessToForceCurve.Evaluate(loudness);
// 		return blowForce;
// 	}
//
//
// 	private bool IsMouseOverCollider()
// 	{
// 		Collider2D collider = GetComponent<Collider2D>();
// 		if (collider == null) return false;
//
// 		// Convert mouse screen position to world position
// 		Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//
// 		// Check if the mouse world position overlaps with the Collider2D
// 		return collider.OverlapPoint(mouseWorldPos);
// 	}
//
// 	public void SetHasPopped(bool p)
// 	{
// 		hasPopped = p;
// 	}
// }

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class BubbleMovement : MonoBehaviour
{
	[Header("Sound Detect Settings")]
	public AudioLoudnessDetection detector;
	public float loudnessSensibility = 100;

	[Header("Bubble Size Settings")]
	public float inflateRate = 0.6f;
	public float shrinkRate = 0.1f;
	public float minSize = 0.3f;
	public float maxSize = 1.5f;
	public float warningThreshold = 0.7f;

	[Header("Bubble Movement Settings")]
	public float baseSpeed = 5f;
	public float sizeFactor = 3f;
	public float minSpeed = 1f;
	public float maxSpeed = 5f;

	[Header("Blow Force Curve")]
	public AnimationCurve loudnessToForceCurve = AnimationCurve.EaseInOut(0f, 0f, 5f, 125f);

	[Header("Additional Settings")]
	public float inertiaDampening = 0.99f;
	public float maxVelocity = 6f;

	[Header("Bubble Wobble Settings")]
	public float wobbleFrequency = 1f;
	public float wobbleAmplitude = 0.3f;
	
	[Header("Bubble Inflate Settings")]
	public float inflateRadius = 0.5f;

	[Header("Debug Info")]
	[SerializeField] private float currentSpeed;
	[SerializeField] private float currentSize;
	[SerializeField] private float currentBlowForce;

	private Rigidbody2D rb;
	private SpriteRenderer spriteRenderer;
	private bool hasPopped = false;
	private float wobbleTime;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();


		rb.gravityScale = 0f;


		rb.drag = 1f;
		// rb.angularDrag = 1f;
	}

	private void Start()
	{

		// transform.localScale = Vector3.one;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			GameManager.Instance.PauseGame();
		}

		if (GameManager.Instance.IsGameRunning && !hasPopped)
		{
			HandleSizeChange();
			HandleColorAndPopCheck();
			HandleBlowInput();
			//HandleWobbleEffect();
		}
	}

	private void FixedUpdate()
	{
		if (GameManager.Instance.IsGameRunning && !hasPopped)
		{
			HandleMovementPhysics();
		}
	}

	#region Bubble Size

	private void HandleSizeChange()
	{
		bool mouseOver = IsMouseOverCollider();


		if (mouseOver && Input.GetKey(KeyCode.Space) && Input.GetMouseButton(0))
		{
			transform.localScale += Vector3.one * inflateRate * Time.deltaTime;
		}
		else
		{
			transform.localScale -= Vector3.one * shrinkRate * Time.deltaTime;
		}

		float clampedSize = Mathf.Clamp(transform.localScale.x, minSize, maxSize);
		transform.localScale = Vector3.one * clampedSize;
		currentSize = clampedSize;
	}

	private void HandleColorAndPopCheck()
	{
		float scale = transform.localScale.x;
		float warningOffset = (maxSize - minSize) * (1f - warningThreshold);

		if (scale >= maxSize)
		{
			ChangeBubbleColor(Color.red);
			if (!hasPopped)
			{
				PopBubble("Bubble popped due to exceeding max size.");
			}
		}
		else if (scale <= minSize)
		{

			ChangeBubbleColor(Color.red);
			if (!hasPopped)
			{
				PopBubble("Bubble popped due to falling below min size.");
			}
		}
		else if (scale >= (maxSize - warningOffset) || scale <= (minSize + warningOffset))
		{
			ChangeBubbleColor(Color.yellow);
			hasPopped = false;
		}
		else
		{
			ChangeBubbleColor(Color.white);
			hasPopped = false;
		}
	}

	private void PopBubble(string reason)
	{
		Debug.Log(reason);
		hasPopped = true;
		GameManager.Instance.EndGame();
		// 这里可实例化爆裂特效、播放音效、Destroy(gameObject) 等
	}

	#endregion

	#region Blow Input

	private float cachedBlowForce = 0f;

	private void HandleBlowInput()
	{
		if (Input.GetKey(KeyCode.Space) && Input.GetMouseButton(0) && !IsMouseOverCollider())
		{
			float loudness = 0f;
			if (detector != null)
			{
				loudness = detector.GetLoudnessFromMicrophone() * loudnessSensibility;
			}
			loudness = Mathf.Clamp(loudness, 0f, 10f);

			// cachedBlowForce = loudnessToForceCurve.Evaluate(loudness);
			cachedBlowForce = 50f;
		}
		else
		{
			cachedBlowForce = 0f;
		}

		currentBlowForce = cachedBlowForce;
	}

	#endregion

	#region Movement Physics

	private void HandleMovementPhysics()
	{

		float scale = transform.localScale.x;
		currentSpeed = baseSpeed + (5f / scale) * sizeFactor;
		currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);

		//    用 ForceMode2D.Force + Time.fixedDeltaTime，可以让浮力相对平滑
		rb.AddForce(Vector2.up * currentSpeed * Time.fixedDeltaTime, ForceMode2D.Force);


		if (cachedBlowForce > 0f)
		{
			Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mouseWorldPos.z = 0f;
			Vector2 directionAway = (transform.position - mouseWorldPos).normalized;


			rb.AddForce(directionAway * cachedBlowForce * Time.fixedDeltaTime, ForceMode2D.Force);
		}


		// rb.velocity *= inertiaDampening;


		if (rb.velocity.magnitude > maxVelocity)
		{
			rb.velocity = rb.velocity.normalized * maxVelocity;
		}
	}

	#endregion

	#region Wobble (Visual Only)

	private void HandleWobbleEffect()
	{
		wobbleTime += Time.deltaTime;
		float wobbleSin = Mathf.Sin(wobbleTime * wobbleFrequency) * wobbleAmplitude;
		float wobbleCos = Mathf.Cos(wobbleTime * wobbleFrequency) * wobbleAmplitude;

		transform.localEulerAngles = new Vector3(0, 0, wobbleSin * 10f);
	}

	#endregion

	#region Helpers

	private void ChangeBubbleColor(Color color)
	{
		if (spriteRenderer != null)
		{
			spriteRenderer.color = color;
		}
	}

 	private bool IsMouseOverCollider()
	{
		Collider2D collider = GetComponent<Collider2D>();
		if (collider == null) return false;

		// Convert mouse screen position to world position
		Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		// Check if any Collider2D within the radius overlaps
		Collider2D[] colliders = Physics2D.OverlapCircleAll(mouseWorldPos, inflateRadius);

		// Return true if the current collider is within the detected colliders
		foreach (Collider2D col in colliders)
		{
			if (col == collider)
			{
				return true;
			}
		}

		return false;
	}

	public void SetHasPopped(bool p)
	{
		hasPopped = p;
	}

	#endregion
}
